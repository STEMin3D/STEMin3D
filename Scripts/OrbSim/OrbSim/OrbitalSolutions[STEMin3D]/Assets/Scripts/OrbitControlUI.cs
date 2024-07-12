using UnityEngine;
using UnityEngine.UI;
using SolarSystem;

public class OrbitControlUI : MonoBehaviour
{
    public Slider yearTSlider;
    public Slider inclinationSlider;
    public Slider argumentOfPerigeeSlider;
    public Slider rightAscensionSlider;

    public SolarSystemManager solarSystemManager;

    void Start()
    {
        if (yearTSlider == null || inclinationSlider == null || argumentOfPerigeeSlider == null || rightAscensionSlider == null)
        {
            Debug.LogError("Please assign all sliders in the Inspector.");
            return;
        }

        if (solarSystemManager == null)
        {
            solarSystemManager = FindObjectOfType<SolarSystemManager>();
        }

        yearTSlider.onValueChanged.AddListener(OnYearTSliderChanged);
        inclinationSlider.onValueChanged.AddListener(OnInclinationSliderChanged);
        argumentOfPerigeeSlider.onValueChanged.AddListener(OnArgumentOfPerigeeSliderChanged);
        rightAscensionSlider.onValueChanged.AddListener(OnRightAscensionSliderChanged);
    }

    void OnYearTSliderChanged(float value)
    {
        solarSystemManager.yearT = value;
    }

    void OnInclinationSliderChanged(float value)
    {
        solarSystemManager.earth.inclination = value;
        solarSystemManager.earth.UpdateOrbit(solarSystemManager.yearT, solarSystemManager.dayT, solarSystemManager.geocentric);
    }

    void OnArgumentOfPerigeeSliderChanged(float value)
    {
        solarSystemManager.earth.argumentOfPerigee = value;
        solarSystemManager.earth.UpdateOrbit(solarSystemManager.yearT, solarSystemManager.dayT, solarSystemManager.geocentric);
    }

    void OnRightAscensionSliderChanged(float value)
    {
        solarSystemManager.earth.rightAscension = value;
        solarSystemManager.earth.UpdateOrbit(solarSystemManager.yearT, solarSystemManager.dayT, solarSystemManager.geocentric);
    }
}
