using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObjectScript : MonoBehaviour
{
    private float au = 5f;
    private float[] rightAscension1;
    private float[] rightAscension2;
    private float[] rightAscension3;
    private float[] declination1;
    private float[] declination2;
    private float[] declination3;
    private float[] distance;
    private float[] absMagnitude;
    private DataSetter dataset;
    public int counter = 0;
    public float raAngle;
    public float dAngle;
    public string celestialName;
    // Start is called before the first frame update
    void Start()
    {
        dataset = GetComponent<DataSetter>();
        dataset.CSVReader(celestialName);
        rightAscension1 = dataset.rightAscension1;
        rightAscension2 = dataset.rightAscension2;
        rightAscension3 = dataset.rightAscension3;
        declination1 = dataset.declination1;
        declination2 = dataset.declination2;
        declination3 = dataset.declination3;
        distance = dataset.distance;
        absMagnitude = dataset.absMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        raAngle = (rightAscension1[counter] + rightAscension2[counter] / 60 + rightAscension3[counter] / 60 / 60) * 15 / 360 * 2 * Mathf.PI;
        dAngle = Mathf.Sign(declination1[counter])*(Mathf.Abs(declination1[counter]) + declination2[counter] / 60 + declination3[counter] / 60 / 60) / 360 * 2 * Mathf.PI;
        float x = distance[counter] * Mathf.Cos(dAngle) * Mathf.Cos(raAngle) * au;
        float y = distance[counter] * Mathf.Cos(dAngle) * Mathf.Sin(raAngle) * au;
        float z = distance[counter] * Mathf.Sin(dAngle) * au;
        transform.position = new Vector3(x, y, z);
        counter++;
        if (counter == rightAscension1.Length)
        {
            counter = 0;
        }
    }
}
