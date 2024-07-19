using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolarSystem
{
    public class EarthOrbit : MonoBehaviour
    {
        public Quaternion earthRot { get; private set; }
        public Vector3 earthPos { get; private set; }
        public float currentAxisAngle { get; private set; }

        public float periapis = 147.2f;
        public float apoapsis = 152.1f;
        public float inclination = 23.4f;
        public float argumentOfPerigee = 0f;
        public float rightAscension = 0f;

        public float distanceScale = 1;

        [Header("Debug")]
        public float debug_dst;

        public Vector3 orbitEllipse { get; private set; }

        public static SolarSystemManager time;

        public Vector2 angle { get; private set; }

        public LineRenderer lineRenderer;
        public int segments = 100;

        public void size(float newSize)
        {
            periapis = newSize;
            apoapsis = newSize;
        }

        public void shapeP(float newPeriapis)
        {
            periapis = newPeriapis;
        }

        public void shapeA(float newApoapsis)
        {
            apoapsis = newApoapsis;
        }

        public void UpdateOrbit(float yearT, float dayT, bool geocentric)
        {
            orbitEllipse = Orbit.CalculatePointOnOrbit(periapis, apoapsis, yearT, inclination, argumentOfPerigee, rightAscension);
            earthPos = orbitEllipse * distanceScale;
            debug_dst = new Vector2(orbitEllipse.x, orbitEllipse.z).magnitude;

            float siderealDayAngle = -dayT * 360;
            float solarDayAngle = siderealDayAngle - yearT * 360;
            currentAxisAngle = solarDayAngle;

            earthRot = Quaternion.Euler(0, 0, -inclination) * Quaternion.Euler(0, currentAxisAngle, 0);

            if (geocentric)
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.position = earthPos;
                transform.rotation = earthRot;
            }

            DrawOrbit();
        }

        private void DrawOrbit()
        {
            Vector3[] points = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / (float)segments;
                Vector3 point = Orbit.CalculatePointOnOrbit(periapis, apoapsis, t, inclination, argumentOfPerigee, rightAscension);
                Vector3 pos = point * distanceScale;
                points[i] = pos;
            }

            points[segments] = points[0];

            lineRenderer.positionCount = segments + 1;
            lineRenderer.SetPositions(points);
        }

        void OnValidate()
        {
            if (Application.isPlaying)
            {
                DrawOrbit();
            }
        }
    }
}
