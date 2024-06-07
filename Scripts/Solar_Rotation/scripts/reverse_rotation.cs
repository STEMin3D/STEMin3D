using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class reverse_rotation : MonoBehaviour
{
    // initiation of objects
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 _rotation;

    // Update is called once per frame
    void Update()
    {
        // sets the sun to rotate counter clockwise matching slider input. neg used to correct direction.
        _rotation.y = -slider.value;
        // actual rotation. multiplies by frame rate so that period calculation and slider value are unimpacted by framerate
        transform.Rotate(_rotation * Time.deltaTime*Application.targetFrameRate);
    }
}
