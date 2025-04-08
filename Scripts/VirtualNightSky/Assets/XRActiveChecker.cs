using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRActiveChecker : MonoBehaviour
{
    public GameObject XROrigin;
    // Start is called before the first frame update
    void Awake()
    {
        XROrigin.SetActive(UnityEngine.XR.XRSettings.enabled);
    }

    // Update is called once per frame
    void Update()
    {
        XROrigin.SetActive(UnityEngine.XR.XRSettings.enabled);
    }
}
