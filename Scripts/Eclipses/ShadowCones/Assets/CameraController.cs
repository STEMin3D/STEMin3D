using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 3f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            float rotateHorizontal = Input.GetAxis ("Mouse X");
		    float rotateVertical = Input.GetAxis ("Mouse Y");
            transform.Rotate(-transform.up * rotateHorizontal * sensitivity);
            transform.Rotate(transform.right * rotateVertical * sensitivity);
        }
    }
}   
