using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class image_rotation : MonoBehaviour
{
    // initiation of objects
    public Texture[] texArray = new Texture[327];
    [SerializeField] private Slider slider;
    Renderer materialRenderer;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        // allows user to change sun's rotation speed, which is controlled by application framerate
        Application.targetFrameRate = (int)slider.value;
        // sets up array of synoptic images that can be applied to sun
        // in an ideal world, these would be numbered starting with 1, but I had to remove a number of images and didn't want to rename the files
        for (int i = 1; i<texArray.Length + 1; i++)
        {
            texArray[i-1] = Resources.Load<Texture>("Textures/s" + (i+173));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // creates a new instance material for the sun and sets it to the next item in texArray
        // texArray items are indexed chronologically so the sun then rotates chronologically
        Material m = GetComponent<Renderer>().material;
        m.mainTexture = texArray[index%327];
        index++;
        // checks to see if sun rotation speed has been adjusted and corrects framerate accordingly
        Application.targetFrameRate = (int)slider.value;
    }
}
