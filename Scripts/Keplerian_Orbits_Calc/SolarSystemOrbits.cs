
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SolarSystemOrbits
/// - Computes heliocentric ecliptic positions from Keplerian orbital elements.
/// - Applies planet spin (sidereal period) with obliquity tilt or IAU pole RA/Dec.
/// - Maps ecliptic XY to Unity XZ (Y-up).
///
/// Recommended data sources:
/// * JPL SSD Horizons for osculating elements at desired epoch (J2000 recommended).
///   https://ssd.jpl.nasa.gov/orbits.html
/// * NASA SSD Planetary Physical Parameters for rotation rates (sidereal periods).
///   https://ssd.jpl.nasa.gov/planets/phys_par.html
/// * IAU WGCCRE for official pole orientations (RA/Dec) and prime meridian definitions.
///   https://www.usgs.gov/centers/astrogeology-science-center/science/iau-wgccre
/// </summary>
public class SolarSystemOrbits : MonoBehaviour
{
    [Header("Global Settings")]
    [Tooltip("Number of Unity units per AU (e.g., 10 means Earth ~10 units from Sun).")]
    public float auToUnity = 10f;

    [Tooltip("Time scale multiplier; 1 = real time (sidereal/mean motion per day).")]
    public double timeScale = 1.0;

    [Tooltip("Reference epoch for the elements (Julian Date), typically J2000 = 2451545.0.")]
    public double epochJD = 2451545.0;

    [Tooltip("Advance time from system clock (UTC). If false, uses simulatedTimeJD below.")]
    public bool useSystemClock = true;

    [Tooltip("Simulated time (JD), used if useSystemClock=false.")]
    public double simulatedTimeJD = 2451545.0;

    [Tooltip("Sun transform (optional). If null, origin is used as Sun location.")]
    public Transform sun;

    [Header("Bodies")]
    public List<BodyConfig> bodies = new List<BodyConfig>();

    // --- Constants ---
    // Gaussian gravitational constant k (AU^(3/2)/day). n = k / a^(3/2) for two-body.
    // mu_sun in AU^3/day^2 = k^2
    private const double K_GAUSS = 0.01720209895;
    private const double MU_SUN_AU3_PER_DAY2 = K_GAUSS * K_GAUSS;             // ~0.000295912208285591
    private const double DEG2RAD = Math.PI / 180.0;
    private const double RAD2DEG = 180.0 / Math.PI;
    // Obliquity of Earth's ecliptic (J2000) for Eq->Ecl conversion when using RA/Dec poles
    private const double EPSILON_J2000_DEG = 23.4392911;

    void Update()
    {
        double jd = useSystemClock ? Astronomy.JulianDateFromDateTime(DateTime.UtcNow) : simulatedTimeJD;

        // Apply time scale (speed up/slow down) relative to real time:
        // convert deltaTime (seconds) -> days, then scale.
        if (useSystemClock == false)
        {
            simulatedTimeJD += Time.deltaTime / 86400.0 * timeScale;
            jd = simulatedTimeJD;
        }
        else
        {
            // You can optionally drift simulated JD relative to system JD with timeScale
            // by keeping an internal accumulator, if desired.
        }

        Vector3 sunPos = sun ? sun.position : Vector3.zero;

        foreach (var b in bodies)
        {
            if (b.planetGO == null) continue;

            // 1) Position from orbital elements
            Vector3 heliocentricUnity = ComputeHeliocentricUnityPosition(b.elements, jd);

            // Offset by Sun's position
            b.planetGO.transform.position = sunPos + heliocentricUnity;

            // 2) Rotation / spin
            Quaternion rot = ComputeBodyOrientation(b, b.elements, jd);
            b.planetGO.transform.rotation = rot;
        }
    }

    /// <summary>
    /// Computes heliocentric ecliptic position, then maps to Unity axes (X,Z,Y) and scales AU -> units.
    /// Ecliptic basis: x-ecliptic, y-ecliptic, z-ecliptic.
    /// Unity mapping: X = x_ecl, Y = z_ecl, Z = y_ecl (so ecliptic plane becomes XZ with Y-up).
    /// </summary>
    private Vector3 ComputeHeliocentricUnityPosition(OrbitalElements el, double jd)
    {
        // Mean motion (rad/day)
        double n_rad_per_day = el.useMeanMotionDegPerDay
            ? el.meanMotionDegPerDay * DEG2RAD
            : Math.Sqrt(MU_SUN_AU3_PER_DAY2 / (el.a_AU * el.a_AU * el.a_AU));

        // Time since epoch (days)
        double dtDays = jd - el.epochJD;

        // Mean anomaly at time (rad)
        double M = Astronomy.NormalizeRadians(el.M0_deg * DEG2RAD + n_rad_per_day * dtDays);

        // Solve Kepler's equation for E
        double E = SolveKeplerE(M, el.e);

        // True anomaly ν
        double nu = 2.0 * Math.Atan2(Math.Sqrt(1 + el.e) * Math.Sin(E / 2.0),
                                     Math.Sqrt(1 - el.e) * Math.Cos(E / 2.0));

        // Distance r (AU)
        double r_AU = el.a_AU * (1.0 - el.e * Math.Cos(E));

        // Position in orbital plane (perifocal PQW)
        double x_p = r_AU * Math.Cos(nu);
        double y_p = r_AU * Math.Sin(nu);
        double z_p = 0.0;

        // Rotate to ecliptic (IJK) using ω, i, Ω (all in radians)
        double cosO = Math.Cos(el.Omega_deg * DEG2RAD);
        double sinO = Math.Sin(el.Omega_deg * DEG2RAD);
        double cosi = Math.Cos(el.i_deg * DEG2RAD);
        double sini = Math.Sin(el.i_deg * DEG2RAD);
        double cosw = Math.Cos(el.omega_deg * DEG2RAD);
        double sinw = Math.Sin(el.omega_deg * DEG2RAD);

        // R3(Ω) * R1(i) * R3(ω) * r_pqw
        double x_ecl = x_p * (cosO * cosw - sinO * sinw * cosi) - y_p * (cosO * sinw + sinO * cosw * cosi);
        double y_ecl = x_p * (sinO * cosw + cosO * sinw * cosi) + y_p * (cosO * cosw * cosi - sinO * sinw);
        double z_ecl = x_p * (sinw * sini) + y_p * (cosw * sini);

        // Map ecliptic to Unity axes and scale
        float X = (float)(x_ecl * auToUnity);
        float Y = (float)(z_ecl * auToUnity); // ecliptic z -> Unity Y (up)
        float Z = (float)(y_ecl * auToUnity);

        return new Vector3(X, Y, Z);
    }

    /// <summary>
    /// Newton-Raphson solver for Kepler's equation: M = E - e sin E (elliptic orbits).
    /// </summary>
    private static double SolveKeplerE(double M, double e)
    {
        // Initial guess: for small e, E ~ M; for larger e, use improved starter
        double E = (e < 0.8) ? M : Math.PI;

        for (int k = 0; k < 20; k++)
        {
            double f = E - e * Math.Sin(E) - M;
            double fp = 1.0 - e * Math.Cos(E);
            double dE = -f / fp;
            E += dE;
            if (Math.Abs(dE) < 1e-12) break;
        }
        return Astronomy.NormalizeRadians(E);
    }

    /// <summary>
    /// Computes body orientation quaternion:
    /// 1) Determines spin axis (either from RA/Dec or from obliquity+orbit).
    /// 2) Builds a base rotation (LookRotation(forward, up)) with up along spin axis.
    /// 3) Applies spin about spin axis using sidereal rotation rate and prime meridian offset.
    /// </summary>
    private Quaternion ComputeBodyOrientation(BodyConfig b, OrbitalElements el, double jd)
    {
        // --- 1) Spin axis in ecliptic coordinates ---
        Vector3 up_ecl;

        if (b.rot.usePoleRADEC) // IAU mode
        {
            // Pole in J2000 Equatorial: convert (RA, Dec) -> unit vector
            Vector3 poleEQ = Astronomy.UnitVectorFromRaDec(b.rot.poleRA_deg, b.rot.poleDec_deg);
            // Convert Equatorial -> Ecliptic via rotation about +X by -epsilon
            up_ecl = Astronomy.RotateAboutX(poleEQ, -(float)EPSILON_J2000_DEG);
        }
        else
        {
            // Orbit normal vector in ecliptic coords: n̂ = (sin i sin Ω, -sin i cos Ω, cos i)
            double i = el.i_deg * DEG2RAD;
            double O = el.Omega_deg * DEG2RAD;
            Vector3 n_orbit = new Vector3(
                (float)(Math.Sin(i) * Math.Sin(O)),
                (float)(-Math.Sin(i) * Math.Cos(O)),
                (float)(Math.Cos(i))
            );

            // Ascending node direction in ecliptic plane: u_nodes = (cos Ω, sin Ω, 0)
            Vector3 u_nodes = new Vector3((float)Math.Cos(O), (float)Math.Sin(O), 0f);

            // Tilt orbit normal about line of nodes by obliquity
            up_ecl = Astronomy.RotateAroundAxis(n_orbit, u_nodes, (float)b.rot.obliquity_deg);
        }

        // Normalize and map to Unity axes (X=x_ecl, Y=z_ecl, Z=y_ecl)
        Vector3 upUnity = new Vector3(up_ecl.x, up_ecl.z, up_ecl.y).normalized;

        // --- 2) Base orientation: choose a 'forward' direction on equator plane ---
        // Project ascending node direction onto the equator (perpendicular to up)
        double Odeg = el.Omega_deg;
        Vector3 nodes_ecl = new Vector3((float)Math.Cos(Odeg * DEG2RAD), (float)Math.Sin(Odeg * DEG2RAD), 0f);
        Vector3 nodesUnity = new Vector3(nodes_ecl.x, nodes_ecl.z, nodes_ecl.y);
        Vector3 nodesProj = Vector3.ProjectOnPlane(nodesUnity, upUnity);
        Vector3 forward = Vector3.Normalize(Vector3.Cross(upUnity, nodesProj));
        if (forward.sqrMagnitude < 1e-6f)
        {
            // Fallback: arbitrary perpendicular
            forward = Vector3.Normalize(Vector3.Cross(upUnity, Vector3.right));
            if (forward.sqrMagnitude < 1e-6f)
                forward = Vector3.Normalize(Vector3.Cross(upUnity, Vector3.forward));
        }

        Quaternion baseRot = Quaternion.LookRotation(forward, upUnity);

        // --- 3) Spin angle θ(t) about 'up' (sidereal) ---
        // Sidereal period can be negative to indicate retrograde (e.g., Venus, Uranus).
        double P_days = b.rot.siderealPeriodHours > 0.0 ? b.rot.siderealPeriodHours / 24.0 : b.rot.siderealPeriodDays;
        if (P_days == 0.0) P_days = 1.0; // avoid div-by-zero
        double omega_rot_deg_per_day = 360.0 / P_days; // sign inherited from P_days (neg for retrograde)
        double dtDays = jd - b.rot.epochJD;
        double thetaDeg = b.rot.theta0_deg + omega_rot_deg_per_day * dtDays;

        // Prime meridian at epoch: rotate by -W0 so the texture's 0° longitude lines up with 'nodesProj' reference.
        float W0 = (float)b.rot.primeMeridianAtEpoch_deg;
        Quaternion primeMeridianOffset = Quaternion.AngleAxis(-W0, upUnity);

        Quaternion spin = Quaternion.AngleAxis((float)thetaDeg, upUnity);
        return spin * baseRot * primeMeridianOffset;
    }
}

[Serializable]
public class BodyConfig
{
    [Header("Object")]
    public GameObject planetGO;

    [Header("Orbital Elements")]
    public OrbitalElements elements = new OrbitalElements();

    [Header("Rotation Parameters")]
    public RotationParams rot = new RotationParams();
}

[Serializable]
public class OrbitalElements
{
    [Tooltip("Semi-major axis (AU).")]
    public double a_AU = 1.0;

    [Tooltip("Eccentricity (0..1).")]
    public double e = 0.0167;

    [Tooltip("Inclination to ecliptic (deg).")]
    public double i_deg = 0.0;

    [Tooltip("Longitude of ascending node Ω (deg).")]
    public double Omega_deg = 0.0;

    [Tooltip("Argument of perihelion ω (deg).")]
    public double omega_deg = 0.0;

    [Tooltip("Mean anomaly at epoch M0 (deg).")]
    public double M0_deg = 0.0;

    [Tooltip("Epoch of elements (JD). Typically J2000 = 2451545.0")]
    public double epochJD = 2451545.0;

    [Tooltip("If true, use mean motion in deg/day; else compute from a using two-body k^2.")]
    public bool useMeanMotionDegPerDay = false;

    [Tooltip("Mean motion (deg/day) if provided by source (e.g., Horizons).")]
    public double meanMotionDegPerDay = 0.0;
}

[Serializable]
public class RotationParams
{
    [Header("Sidereal Rotation")]
    [Tooltip("Sidereal rotation period in hours (use positive for prograde; negative for retrograde). If zero, uses days field.")]
    public double siderealPeriodHours = 0.0;

    [Tooltip("Sidereal rotation period in days (alternative to hours).")]
    public double siderealPeriodDays = 0.0;

    [Header("Axis Orientation")]
    [Tooltip("Use IAU pole RA/Dec (preferred if available). If false, use obliquity tilt from orbit normal.")]
    public bool usePoleRADEC = false;

    [Tooltip("IAU pole right ascension α (deg, J2000).")]
    public double poleRA_deg = 0.0;

    [Tooltip("IAU pole declination δ (deg, J2000).")]
    public double poleDec_deg = 90.0;

    [Tooltip("Obliquity (deg): tilt of spin axis from orbit normal (used if usePoleRADEC=false).")]
    public double obliquity_deg = 23.4392911;

    [Header("Meridian / Spin Phase")]
    [Tooltip("Prime meridian angle at epoch W0 (deg). Use IAU value if available, else leave 0.")]
    public double primeMeridianAtEpoch_deg = 0.0;

    [Tooltip("Spin phase offset at epoch θ0 (deg). Adjust to align texture features visually if needed.")]
    public double theta0_deg = 0.0;

    [Tooltip("Epoch (JD) used to define θ0 and W0, usually same as orbital epoch (e.g., J2000).")]
    public double epochJD = 2451545.0;
}

public static class Astronomy
{
    /// <summary>
    /// Julian Date from DateTime (UTC). Valid for Gregorian dates.
    /// </summary>
    public static double JulianDateFromDateTime(DateTime utc)
    {
        // Algorithm from USNO/NREL; assumes Gregorian calendar.
        int Y = utc.Year;
        int M = utc.Month;
        double D = utc.Day + (utc.Hour + (utc.Minute + utc.Second / 60.0) / 60.0) / 24.0;

        if (M <= 2) { Y -= 1; M += 12; }

        int A = Y / 100;
        int B = 2 - A + (A / 4);

        double JD = Math.Floor(365.25 * (Y + 4716))
                  + Math.Floor(30.6001 * (M + 1))
                  + D + B - 1524.5;

        return JD;
    }

    /// <summary>Normalize angle to [0, 2π).</summary>
    public static double NormalizeRadians(double x)
    {
        x %= (2.0 * Math.PI);
        if (x < 0) x += 2.0 * Math.PI;
        return x;
    }

    /// <summary>Normalize angle to [0°, 360°).</summary>
    public static double NormalizeDegrees(double x)
    {
        x %= 360.0;
        if (x < 0) x += 360.0;
        return x;
    }

    /// <summary>Unit vector from RA (deg), Dec (deg) in J2000 equatorial frame.</summary>
    public static Vector3 UnitVectorFromRaDec(double raDeg, double decDeg)
    {
        double ra = raDeg * Math.PI / 180.0;
        double dec = decDeg * Math.PI / 180.0;
        double cosDec = Math.Cos(dec);
        return new Vector3(
            (float)(Math.Cos(ra) * cosDec),
            (float)(Math.Sin(ra) * cosDec),
            (float)(Math.Sin(dec))
        );
    }

    /// <summary>Rotate vector about +X axis by angleDeg.</summary>
    public static Vector3 RotateAboutX(Vector3 v, float angleDeg)
    {
        float a = angleDeg * Mathf.Deg2Rad;
        float ca = Mathf.Cos(a), sa = Mathf.Sin(a);
        return new Vector3(
            v.x,
            ca * v.y - sa * v.z,
            sa * v.y + ca * v.z
        );
    }

    /// <summary>Axis-angle rotation using Rodrigues' formula.</summary>
    public static Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float angleDeg)
    {
        Vector3 k = axis.normalized;
        float a = angleDeg * Mathf.Deg2Rad;
        float ca = Mathf.Cos(a), sa = Mathf.Sin(a);
        return v * ca + Vector3.Cross(k, v) * sa + k * Vector3.Dot(k, v) * (1f - ca);
    }
}
