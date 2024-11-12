using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ConstellationFinder : MonoBehaviour
{
    public string[] names;
    public float[] rightAscension1;
    public float[] rightAscension2;
    public float[] rightAscension3;
    public float[] declination1;
    public float[] declination2;
    public float[] declination3;
    private int distance = 975;
    private TextAsset rawData;
    private float raAngle;
    private float dAngle;
    private string[] data;
    private GameObject Label;
    private GameObject[] labelArray;
    // Start is called before the first frame update
    void Awake()
    {
        ReadData();
        Label = Resources.Load<GameObject>("Label");
        labelArray = new GameObject[data.Length];
        for (int i = 0; i<data.Length; i++)
        {
            raAngle = -(rightAscension1[i] + rightAscension2[i] / 60 + rightAscension3[i] / 60 / 60) * 15 / 360 * 2 * Mathf.PI;
            dAngle = Mathf.Sign(declination1[i]) * (Mathf.Abs(declination1[i]) + declination2[i] / 60 + declination3[i] / 60 / 60) / 360 * 2 * Mathf.PI;
            float x = distance * Mathf.Cos(dAngle) * Mathf.Cos(raAngle);
            float y = distance * Mathf.Cos(dAngle) * Mathf.Sin(raAngle);
            float z = distance * Mathf.Sin(dAngle);
            GameObject newLabel = (GameObject)Instantiate(Label, new Vector3(x, y, z), Quaternion.identity);
            newLabel.transform.LookAt(new Vector3(0, 0, 0));
            newLabel.transform.rotation = transform.rotation;
            newLabel.GetComponentInChildren<TextMeshProUGUI>().text = names[i];
            labelArray[i] = newLabel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i<labelArray.Length; i++)
        {
            labelArray[i].transform.LookAt(new Vector3(0, 0, 0));
            labelArray[i].transform.rotation = transform.rotation;
        }
    }

    void ReadData()
    {
        rawData = Resources.Load("constellations") as TextAsset;
        data = rawData.text.Split(new String[] { ",", "" }, StringSplitOptions.None);
        string[] temp1 = new string[data.Length];
        float[] temp2 = new float[data.Length];
        float[] temp3 = new float[data.Length];
        float[] temp4 = new float[data.Length];
        float[] temp5 = new float[data.Length];
        float[] temp6 = new float[data.Length];
        float[] temp7 = new float[data.Length];
        for (int i = 0; i< data.Length; i++)
        {
            string stellInfo = data[i];
            for (int j = 0; j<7; j++)
            {
                if (j == 0)
                {
                    string checkString = stellInfo.Substring(0, stellInfo.IndexOf(" "));
                    if (checkString.IndexOf("_")!=-1)
                    {
                        checkString = checkString.Substring(0, checkString.IndexOf("_")) + " " + checkString.Substring(checkString.IndexOf("_") + 1, stellInfo.IndexOf(" ")-checkString.IndexOf("_")-1);
                    }
                    temp1[i] = checkString;
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length-stellInfo.IndexOf(" ")-1);
                }
                if (j == 1)
                {
                    temp2[i] = float.Parse(stellInfo.Substring(0, stellInfo.IndexOf(" ")));
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length - stellInfo.IndexOf(" ")-1);
                }
                if (j == 2)
                {
                    temp3[i] = float.Parse(stellInfo.Substring(0, stellInfo.IndexOf(" ")));
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length - stellInfo.IndexOf(" ")-1);
                }
                if (j == 3)
                {
                    temp4[i] = float.Parse(stellInfo.Substring(0, stellInfo.IndexOf(" ")));
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length - stellInfo.IndexOf(" ")-1);
                }
                if (j == 4)
                {
                    temp5[i] = float.Parse(stellInfo.Substring(0, stellInfo.IndexOf(" ")));
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length - stellInfo.IndexOf(" ")-1);
                }
                if (j == 5)
                {
                    temp6[i] = float.Parse(stellInfo.Substring(0, stellInfo.IndexOf(" ")));
                    stellInfo = stellInfo.Substring(stellInfo.IndexOf(" ")+1, stellInfo.Length - stellInfo.IndexOf(" ")-1);
                }
                if (j == 6)
                {
                    temp7[i] = float.Parse(stellInfo);
                }
            }
        }
        names = temp1;
        rightAscension1 = temp2;
        rightAscension2 = temp3;
        rightAscension3 = temp4;
        declination1 = temp5;
        declination2 = temp6;
        declination3 = temp7;
    }
}
