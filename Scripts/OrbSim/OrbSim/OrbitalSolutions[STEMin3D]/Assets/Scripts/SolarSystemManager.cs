using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolarSystem
{
    [ExecuteInEditMode]
    public class SolarSystemManager : MonoBehaviour
    {
        public bool animate;

        [Header("Durations")]
        public float dayDurationMinutes;
        public float monthDurationMinutes;
        public float yearDurationMinutes;

        [Header("References")]
        public Sun sun;
        public EarthOrbit earth;
        public Transform player;

        [Header("Time state")]
        [Range(0, 1)]
        public float dayT;
        [Range(0, 1)]
        public float monthT;
        [Range(0, 1)]
        public float yearT;

        public float fastForwardDayDuration;
        float oldPlayerT;
        float fastForwardTargetTime;
        bool fastForwardApproachingTargetTime;

        [Header("Debug")]
        public bool geocentric;

        void Update()
        {
            // Update the orbits
            earth?.UpdateOrbit(yearT, dayT, geocentric);
            sun?.UpdateOrbit(earth, geocentric);
        }

        public void SetTimes(float dayT, float monthT, float yearT)
        {
            this.dayT = dayT;
            this.monthT = monthT;
            this.yearT = yearT;
        }

        float CalculatePlayerDayT()
        {
            return Vector3.Dot(player.position.normalized, -sun.transform.forward);
        }

        float DstToTargetTime(float fromT, float targetT)
        {
            return Mathf.Abs(targetT - fromT);
        }
    }
}
