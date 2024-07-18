using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public float sun_radius_scaling = 350000;
    public float sunearth_distance_scaling = 12416666;
    public float earth_radius_scaling = 6378;
    public float earthmoon_distance_scaling = 12100;
    public float moon_radius_scaling = 3480;
    
    public GameObject sun;
    public GameObject earth;
    public GameObject moon;

    public Transform light;
    
    public float moonRotationSpeed = 100;
    public float earthRotationSpeed = 5;
    public float timeScale = 1;

    private float sunradius = 700000;
    private float earthradius = 6378;
    float moonradius = 1740;
    float earthsundist = 149000000;
    float earthmoondist = 38400;
    // Start is called before the first frame update
    void Start()
    {
        // Uncomment this for proper scaling
        
        sun.transform.localScale= new Vector3(sunradius/sun_radius_scaling,sunradius/sun_radius_scaling,sunradius/sun_radius_scaling);
        earth.transform.localScale= new Vector3(earthradius/earth_radius_scaling,earthradius/earth_radius_scaling,earthradius/earth_radius_scaling);
        moon.transform.localScale= new Vector3(moonradius/moon_radius_scaling,moonradius/moon_radius_scaling,moonradius/moon_radius_scaling);
        sun.transform.localPosition= new Vector3(0,0,0);
        earth.transform.localPosition = new Vector3(sunradius/sun_radius_scaling+earthsundist/sunearth_distance_scaling+earthradius/earth_radius_scaling,0,0);
        moon.transform.localPosition = new Vector3(earth.transform.localPosition.x+earthradius/earth_radius_scaling+earthmoondist/earthmoon_distance_scaling+moonradius/moon_radius_scaling,0,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(earth.transform);
        //transform.Translate(Vector3.right*Time.deltaTime*2);
        Orbit(sun, earth, earthRotationSpeed);
        Orbit(earth, moon, moonRotationSpeed);
        light.LookAt(earth.transform);
    }

    void Orbit(GameObject source, GameObject orbiter, float speed)
    {
        orbiter.transform.RotateAround(source.transform.position, Vector3.up, speed*Time.deltaTime*timeScale);
        orbiter.transform.Rotate(Vector3.up*speed*Time.deltaTime*timeScale);
    }

    public void SetTimeScale(float ts)
    {
        timeScale = ts;
    }
    
}

