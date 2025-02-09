using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // Start is called before the first frame update
    void Start()
    {
        cfinder = Camera.main.GetComponent<ConstellationFinder>();
        simmer.onClick.AddListener(switchSim);
        fielder.onClick.AddListener(switchField);
        transform.LookAt(new Vector3(0,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        if(simIsOn)
        {
            transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * 10f);
        }
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
