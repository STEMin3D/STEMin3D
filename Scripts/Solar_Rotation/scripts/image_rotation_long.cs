using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class image_rotation_long : MonoBehaviour
{
    // initiation
    public Texture[] texArray = new Texture[136];
    private Renderer materialRenderer;
    private int index = 0;
    private int counter = 0;

    void Start()
    {
        // sets frame rate (thereby setting rotation speed of sun since images update each frame)
        Application.targetFrameRate = 20;
        // sets up array of synoptic images that can be applied to sun (l = long)
        for (int i = 1; i<texArray.Length + 1; i++)
        {
            texArray[i-1] = Resources.Load<Texture>("Textures/l" + (i));
        }
    }

    void Update()
    {
        // counter is used so that images dont update every frame since long sim has more drastic changes
        if (counter%8==0)
        {
        // creates a new instance material for the sun and sets it to the next item in texArray
        // texArray items are indexed chronologically so the sun then rotates chronologically
            Material m = GetComponent<Renderer>().material;
            m.mainTexture = texArray[(index)%136];
            index++;
        }
        counter++;
    }
}
