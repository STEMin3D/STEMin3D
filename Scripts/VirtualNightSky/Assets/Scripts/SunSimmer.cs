using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class SunSimmer : MonoBehaviour
{
    public Button simmer;
    public Button fielder;
    private bool simIsOn = false;
    private bool fieldOn = true;
    public GameObject theLight;
    public GameObject theField;
    private ConstellationFinder cfinder;
    private HDAdditionalLightData lighter;
    // Start is called before the first frame update
    void Start()
    {
        lighter = theLight.GetComponent<HDAdditionalLightData>();
        cfinder = Camera.main.GetComponent<ConstellationFinder>();
        simmer.onClick.AddListener(switchSim);
        fielder.onClick.AddListener(switchField);
        transform.LookAt(new Vector3(0,0,0));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(simIsOn)
        {
            transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * 10f);
        }
        //I have this code temporarily commented out, may return to it later to simulate sunrise/sundown better
        //Debug.Log(theLight.transform.localEulerAngles.x + " " + theLight.transform.localEulerAngles.y + " " + theLight.transform.localEulerAngles.z);
        //if(theLight.transform.localEulerAngles.x <= 20 && theLight.transform.localEulerAngles.z > 160)
        //{
        //    light.intensity = Mathf.Pow(10, theLight.transform.localEulerAngles.x / 5f) + 1000;
        //}
    }

    void switchSim()
    {
        simIsOn = !simIsOn;
    }

    void switchField()
    {
        fieldOn = !fieldOn;
        cfinder.LabelActivation(fieldOn);
        theField.SetActive(fieldOn);
    }
}
