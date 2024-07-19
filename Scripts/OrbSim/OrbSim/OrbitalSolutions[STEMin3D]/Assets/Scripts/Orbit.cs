using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

namespace SolarSystem
{
    public class Orbit : MonoBehaviour
    {
        // Calculate point on orbit at t (going from 0 at the start of the orbit, to 1 at the end of the orbit)
        // The orbit is defined by the periapsis (distance of closest approach) and apoapsis (farthest distance)
        public static Vector3 CalculatePointOnOrbit(double periapsis, double apoapsis, double t, double inclination, double argumentOfPerigee, double rightAscension)
        {
            double semiMajorLength = (apoapsis + periapsis) / 2;
            double linearEccentricity = semiMajorLength - periapsis; // distance between centre and focus
            double eccentricity = linearEccentricity / semiMajorLength; // (0 = perfect circle, and up to 1 is increasingly elliptical)
            double semiMinorLength = Sqrt(Pow(semiMajorLength, 2) - Pow(linearEccentricity, 2));

            double meanAnomaly = t * PI * 2;
            double eccentricAnomaly = SolveKepler(meanAnomaly, eccentricity);

            // Position in the plane of the ellipse
            double ellipseCentreX = -linearEccentricity;
            double pointX = Cos(eccentricAnomaly) * semiMajorLength + ellipseCentreX;
            double pointY = Sin(eccentricAnomaly) * semiMinorLength;

            Vector3 position = new Vector3((float)pointX, 0, (float)pointY);

            // Apply rotations for argument of perigee, inclination, and right ascension of the ascending node (RAAN)
            Quaternion rotation = Quaternion.Euler(0, (float)rightAscension, 0) *
                                  Quaternion.Euler((float)inclination, 0, 0) *
                                  Quaternion.Euler(0, (float)argumentOfPerigee, 0);

            return rotation * position;
        }

        static double SolveKepler(double meanAnomaly, double eccentricity, int maxIterations = 100)
        {
            const double h = 0.0001; // step size for approximating gradient of the function
            const double acceptableError = 0.00000001;
            double guess = meanAnomaly;

            for (int i = 0; i < maxIterations; i++)
            {
                double y = KeplerEquation(guess, meanAnomaly, eccentricity);
                if (Abs(y) < acceptableError)
                {
                    break;
                }
                double slope = (KeplerEquation(guess + h, meanAnomaly, eccentricity) - y) / h;
                double step = y / slope;
                guess -= step;
            }
            return guess;

            double KeplerEquation(double E, double M, double e)
            {
                return M - E + e * Sin(E);
            }
        }
    }
}
