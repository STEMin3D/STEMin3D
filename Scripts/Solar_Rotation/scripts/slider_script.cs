using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class slider_script : MonoBehaviour
{
    // initiation of objects
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    // Start is called before the first frame update
    void Start()
    {
        // sets initial period calculation and puts on UI
        slider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("Period: " + 360/slider.value);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        // updates period calculation. Note since rotation speed is calculated w/ framerate this doesnt need adjustment
        sliderText.text = "Period: " + 360/slider.value;
    }
}
