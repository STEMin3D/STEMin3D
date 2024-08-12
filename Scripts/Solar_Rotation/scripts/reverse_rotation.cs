using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class reverse_rotation : MonoBehaviour
{
    // initiation
    public Transform sun;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider radius;
    [SerializeField] private Vector3 _rotation;

    // sets value of radius on hidden radius slider, which controls how far camera is from sun as camera moves
    void Start()
    {
        radius.value = Math.Abs(transform.position.z);
    }

    void Update()
    {
        //checks that camera is looking at sun
        transform.LookAt(sun);
        //calculates angle and height needed that allows camera to properly readjust position after camera rotates upward
        float theta = Mathf.Atan(transform.position.y/(float)Math.Sqrt(Math.Pow(transform.position.z, 2)+Math.Pow(transform.position.x, 2)));
        float heightradius = (float)Math.Sqrt(Math.Pow(transform.position.z, 2)+Math.Pow(transform.position.x, 2));
        _rotation.z = 0;
        _rotation.x = -slider.value;
        // actual rotation of camera, multiplies by frame rate so that period calculation and slider value are unimpacted by framerate
        // also adds a scaling factor that adjusts camera rotation speed to always equal sun's rotation speed even when camera zooms in/out
        transform.Translate(_rotation*Time.deltaTime*(float)Math.Sqrt(Math.Pow(transform.position.z, 2)+Math.Pow(transform.position.x, 2))/3);
        transform.LookAt(sun);

        // if camera is too close/far, adjusts camera
        if (Math.Sqrt(Math.Pow(transform.position.z, 2)+Math.Pow(transform.position.x, 2)) > heightradius)
        {
            // makes adjustment using angle and height from before
            _rotation.x = 0;
            float move = (float)Math.Sqrt(Math.Pow(transform.position.z, 2)+Math.Pow(transform.position.x, 2)) - heightradius;
            _rotation.y = move*Mathf.Sin(theta);
            _rotation.z = move*Mathf.Cos(theta);
            transform.Translate(_rotation);
            transform.LookAt(sun);

            //further adjustment if too far/close
            if (transform.position.magnitude > Math.Abs(radius.value))
            {
                _rotation.y = 0;
                _rotation.z = Math.Abs(transform.position.magnitude) - radius.value;
                transform.Translate(_rotation);
            }
        }
        transform.LookAt(sun);
    }
}
