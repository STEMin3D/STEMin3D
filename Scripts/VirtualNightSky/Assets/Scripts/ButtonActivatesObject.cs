using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivatesObject : MonoBehaviour
{
    public Button theButton;
    public GameObject theObject;
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        theObject.SetActive(isActive);
        theButton.onClick.AddListener(Activation);
    }

    void Activation()
    {
        isActive = !isActive;
        theObject.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
