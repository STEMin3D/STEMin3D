using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class image_rotation : MonoBehaviour
{
    // initiation
    public Texture[] texArray = new Texture[327];
    private Renderer materialRenderer;
    private int index = 0;

    void Start()
    {
        // sets frame rate (thereby setting rotation speed of sun since images update each frame)
        Application.targetFrameRate = 20;
        // sets up array of synoptic images that can be applied to sun (s = short)
        // in an ideal world, these would be numbered starting with 1, but I had to remove a number of images and didn't want to rename the files
        for (int i = 1; i<texArray.Length + 1; i++)
        {
            texArray[i-1] = Resources.Load<Texture>("Textures/s" + (i+173));
        }
    }

    void Update()
    {
        // creates a new instance material for the sun and sets it to the next item in texArray
        // texArray items are indexed chronologically so the sun then rotates chronologically
            Material m = GetComponent<Renderer>().material;
            m.mainTexture = texArray[(index)%327];
        index++;
    }
}
