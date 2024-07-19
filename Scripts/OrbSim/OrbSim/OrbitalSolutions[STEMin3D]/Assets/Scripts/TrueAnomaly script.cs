using UnityEngine;
using UnityEngine.UI;
using SolarSystem;

public class YearTSliderController : MonoBehaviour
{
    public Slider yearTSlider;
    public SolarSystemManager solarSystemManager;

    void Start()
    {
        if (yearTSlider == null)
        {
            yearTSlider = GetComponent<Slider>();
        }

        if (solarSystemManager == null)
        {
            // Optionally find the SolarSystemManager in the scene if not assigned
            solarSystemManager = FindObjectOfType<SolarSystemManager>();
        }

        // Add listener to call the OnSliderValueChanged method when the slider value changes
        yearTSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Method to update the yearT value in the SolarSystemManager
    void OnSliderValueChanged(float value)
    {
        if (solarSystemManager != null)
        {
            solarSystemManager.yearT = value;
        }
    }
}
