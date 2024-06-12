using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class camera_movement : MonoBehaviour
{
    // initiation of objects
    public Transform sun;
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    private Vector3 rotation = Vector3.zero;
    private Vector3 zoom = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        // sets the camera to look at the sun, so when x/y/z changed it rotates camera about sun
        transform.LookAt(sun);
        // allows user to manually control the zooming and rotation
        rotation.y = slider1.value;
        zoom.z = slider2.value;
        // sets rotation
        transform.Translate(rotation*Time.deltaTime);
        // sets zoom. magnitude limitations used to prevent from zooming too far out or zooming inside sun's surface
        if (transform.position.magnitude>=1.6&&transform.position.magnitude<=3)
        {
            transform.Translate(zoom*Time.deltaTime);
        }
        if (transform.position.magnitude<1.6&&zoom.z==-1)
        {
            transform.Translate(zoom*Time.deltaTime);
        }
        if (transform.position.magnitude>3&&zoom.z==1)
        {
            transform.Translate(zoom*Time.deltaTime);
        }
    }
}
