using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMaker : MonoBehaviour
{
    public GameObject sun;
    public GameObject earth;
    public GameObject moon;
    public GameObject conePrefab;
    private GameObject earthCone;
    private GameObject moonCone;
    
    // Start is called before the first frame update
    void Start()
    {
        earthCone = Instantiate(conePrefab,sun.transform.position, Quaternion.identity);
        moonCone = Instantiate(conePrefab,sun.transform.position, Quaternion.identity);
        ToggleEclipse(true);
    }

    // Update is called once per frame
    void Update()
    {
       UpdateCone(earthCone, earth);
       UpdateCone(moonCone, moon);
    }

    void UpdateCone(GameObject cone, GameObject target)
    {
        cone.transform.position = new Vector3(
            sun.transform.position.x,
            sun.transform.position.y,
            sun.transform.position.z
        );
        cone.transform.localScale = new Vector3(
            sun.transform.localScale.x*2,
            sun.transform.localScale.y*2,
            GetConeLength(sun.transform.localScale.x,target.transform.localScale.x, Vector3.Distance(sun.transform.position, target.transform.position))
        );
        cone.transform.LookAt(target.transform);
    }
    
    float GetConeLength(float r1, float r2, float d)
    {
        return r1*d/(r1+r2);
    }

    public void ToggleEclipse(bool isSolar)
    {
        moonCone.SetActive(isSolar);
        earthCone.SetActive(!isSolar);
    }
}
