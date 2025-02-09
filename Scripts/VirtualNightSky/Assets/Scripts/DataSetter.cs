using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataSetter : MonoBehaviour
{
    public float[] rightAscension1;
    public float[] rightAscension2;
    public float[] rightAscension3;
    public float[] declination1;
    public float[] declination2;
    public float[] declination3;
    public float[] distance;
    public float[] absMagnitude;
    public TextAsset dataTable;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 20;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CSVReader(String body)
    {
        dataTable = Resources.Load(body) as TextAsset;
        String[] data = dataTable.text.Split(new String[] { ",", "\n" }, StringSplitOptions.None);
        float[] temp1 = new float[data.Length / 4];
        float[] temp2 = new float[data.Length / 4];
        float[] temp3 = new float[data.Length / 4];
        float[] temp4 = new float[data.Length / 4];
        float[] temp5 = new float[data.Length / 4];
        float[] temp6 = new float[data.Length / 4];
        float[] temp7 = new float[data.Length / 4];
        float[] temp8 = new float[data.Length / 4];
        for (int i = 0; i < data.Length; i++)
        {
            if (i%4==0)
            {
                int counter = 0;
                int index1 = 0; ;
                string ascension = data[i];
                for (int j = 0; j < ascension.Length; j++)
                {
                    if (ascension.Substring(j, 1).Equals(" "))
                    {
                        if (counter == 0)
                        {
                            temp1[i/4] = float.Parse(ascension.Substring(0, j));
                            index1 = j;
                        }
                        if (counter == 1)
                        {
                            temp2[i/4] = float.Parse(ascension.Substring(index1 + 1, j-index1-1));
                            temp3[i/4] = float.Parse(ascension.Substring(j + 1));
                        }
                        counter++;
                    }
                }
            }
            if (i%4==1)
            {
                int counter = 0;
                int index1 = 0; ;
                string declination = data[i];
                for (int j = 0; j < declination.Length; j++)
                {
                    if (declination.Substring(j, 1).Equals(" "))
                    {
                        if (counter == 0)
                        {
                            temp4[i / 4] = float.Parse(declination.Substring(0, j));
                            index1 = j;
                        }
                        if (counter == 1)
                        {
                            temp5[i / 4] = float.Parse(declination.Substring(index1 + 1, j-index1-1));
                            temp6[i / 4] = float.Parse(declination.Substring(j + 1));
                        }
                        counter++;
                    }
                }
            }
            if (i%4==2)
            {
                temp7[i / 4] = float.Parse(data[i]);
            }
            if (i%4==3)
            {
                temp8[i / 4] = float.Parse(data[i]);
            }
        }
        rightAscension1 = temp1;
        rightAscension2 = temp2;
        rightAscension3 = temp3;
        declination1 = temp4;
        declination2 = temp5;
        declination3 = temp6;
        distance = temp7;
        absMagnitude = temp8;
    }
}
