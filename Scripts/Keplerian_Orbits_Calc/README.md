## Calculate accurate planetary orbits by using `orbital elements`.

Sources for parameters (recommended for realistic values):
 
- Orbital elements & ephemerides: NASA/JPL SSD Horizons (most accurate, configurable output).
  - https://ssd.jpl.nasa.gov/orbits.html (Horizons overview)
- Rotation periods/obliquities & physical params: NASA SSD Planetary Physical Parameters; IAU WGCCRE reports.
  - https://ssd.jpl.nasa.gov/planets/phys_par.html (rotation periods, basic facts)
  - https://www.usgs.gov/centers/astrogeology-science-center/science/iau-wgccre (official pole orientations)
 
### 🛠️ How to Use

1. Create an empty GameObject (e.g., SolarSystem) and attach SolarSystemOrbits.cs.
2. Assign the Sun Transform (optional). If omitted, origin (0,0,0) is taken as the Sun.
3. For each planet:
  - Add an entry in Bodies and assign the planet GameObject (with the textured sphere).
  - Fill OrbitalElements using a consistent epoch—ideally J2000 (JD 2451545.0).
    - If Horizons gives mean motion (deg/day), set useMeanMotionDegPerDay = true and paste it.
    - Otherwise set a (AU) and it will compute n from two-body dynamics.
  - Fill RotationParams:
    - Preferred: set usePoleRADEC = true and paste IAU pole RA/Dec (J2000).
      (See IAU WGCCRE tables; they provide official axes.)
    - Otherwise: set usePoleRADEC = false and paste sidereal period and obliquity.
    - For retrograde rotations (e.g., Venus, Uranus), use a negative period (e.g., siderealPeriodDays = -0.71833 for Uranus).
    - If you have prime meridian at epoch (W₀), paste into primeMeridianAtEpoch_deg to line up textures precisely.
4. Scaling:
  - Set auToUnity (e.g., 10) to place Earth at ~10 units from the Sun. Adjust to avoid float precision issues.
  - Consider non-linear visual scaling for distances and radii if you want a pedagogical, not-to-scale model.
5. Time:
  - useSystemClock = true → positions update with real UTC.
  - useSystemClock = false → sim runs from simulatedTimeJD and advances by timeScale × deltaTime.

### Notes & Accuracy Tips
- Sources & fidelity: For the most accurate positions (including perturbations), query JPL Horizons for either:
  - Osculating orbital elements at your epoch (then use this script), or
  - Cartesian state vectors (position/velocity) and bypass two-body assumptions for long spans or high precision.
    Source: NASA/JPL SSD Horizons overview → https://ssd.jpl.nasa.gov/orbits.html
- Rotation axes: When available, use IAU pole RA/Dec (per WGCCRE) rather than deriving from obliquity to get exact axis orientations and prime-meridian behavior.
  References: NASA SSD planetary physical parameters (rotation periods), IAU WGCCRE reports →
  https://ssd.jpl.nasa.gov/planets/phys_par.html, https://www.usgs.gov/centers/astrogeology-science-center/science/iau-wgccre
- Texture alignment: Planet maps may expect a particular zero-longitude. Use primeMeridianAtEpoch_deg (W₀). If you don’t have W₀, adjust theta0_deg visually to align notable features (e.g., Olympus Mons on Mars).
- Performance: This script uses doubles for math and updates per frame—fine for 9 bodies. For dozens/hundreds, cache results or update at fixed intervals.

### 📦 Example (Earth) — values for illustration only
Populate via Inspector:

- OrbitalElements (J2000):
  a_AU = 1.00000261, e = 0.01671123, i_deg = 0.00005, Omega_deg = -11.26064, omega_deg = 102.93768193, M0_deg = 100.46457166, epochJD = 2451545.0
- RotationParams:
  usePoleRADEC = true, poleRA_deg = 0.00, poleDec_deg = 90.00 (Earth’s axis in ICRS—replace with exact WGCCRE values)
  siderealPeriodDays = 0.99726968, primeMeridianAtEpoch_deg = 0, theta0_deg = 0, epochJD = 2451545.0
For authoritative numbers, query Horizons for Earth elements at J2000 or your chosen epoch, and use IAU WGCCRE for pole and meridian.
