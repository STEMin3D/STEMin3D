using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomCompassControls : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject North;
    public GameObject West;
    public GameObject South;
    public GameObject East;

    public GameObject NorthWest;
    public GameObject NorthEast;
    public GameObject SouthWest;
    public GameObject SouthEast;

    public GameObject compassFace;

    public GameObject mainCam;
    float currentRotation;
    float newRotation;
    GameObject[] elements;
    void Start()
    {
        currentRotation = mainCam.transform.localEulerAngles.y;
        elements = new GameObject[] { North, West, South, East, NorthWest, NorthEast, SouthWest, SouthEast, compassFace };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newRotation = mainCam.transform.localEulerAngles.y;
        if (currentRotation!=newRotation)
        {
            transform.Rotate(0, 0, newRotation - currentRotation, Space.Self);
            for (int i  = 0; i < elements.Length; i++)
            {
                if (i != elements.Length-1)
                {
                    elements[i].transform.Rotate(0, 0, currentRotation - newRotation, Space.Self);
                }
            }
        }
        currentRotation = newRotation;
    }
}
