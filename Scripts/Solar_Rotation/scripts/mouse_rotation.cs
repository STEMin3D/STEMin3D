using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class mouse_rotation : MonoBehaviour
{
    // initiation
    public Transform sun;
    private Vector3 mposition1;
    private Vector3 mposition2;
    private Vector3 rotating;
    [SerializeField] private Slider radius;

    //initializes the mouse position
    void Start()
    {
        mposition1 = Input.mousePosition;
    }

    void Update()
    {
        // makes sure camera is looking at sun
        transform.LookAt(sun);
        // checks if mouse right click is being held and if camera is not too high or too low, if yes runs
        if(Input.GetMouseButton(1)&&Math.Abs(transform.position.y)<=radius.value+0.2)
        {
            // finds difference in mouse positions and then rotates based on magnitude of difference
            mposition2 = mposition1;
            mposition1 = Input.mousePosition;
            rotating = (mposition2 - mposition1);
            rotating.x = 0;
            rotating.z = 0;
            transform.Translate(rotating*Time.deltaTime/10);
            
            // checks to see if rotation moved camera too far/close to sun than preset value and adjusts
            if (transform.position.magnitude > Math.Abs(radius.value))
            {
                rotating.y = 0;
                rotating.z = Math.Abs(transform.position.magnitude) - radius.value;
                transform.Translate(rotating*Time.deltaTime);
            }
        }
        //if no, updates mouse position anyways
        else
        {
            mposition1 = Input.mousePosition;
        }
        //checks to see if MMB has been changed, if yes runs
        if(Input.mouseScrollDelta.y!=0)
        {
            // evaluates MMB change and moves camera in/out accordingly
            radius.value = radius.value - Input.mouseScrollDelta.y/10;
            rotating.x = 0;
            rotating.y = 0;
            rotating.z = Math.Abs(transform.position.magnitude) - radius.value;
            transform.Translate(rotating);
        }
    }
}
