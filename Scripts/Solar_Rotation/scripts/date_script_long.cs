using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class date_script_long : MonoBehaviour
{
    // initiation
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI scrollbarText;
    private int index = 0;
    private DateTime start = new DateTime(2011, 9, 22);
    private DateTime now = new DateTime();
    private DateTime then = new DateTime();
    private TimeSpan day = new TimeSpan(24, 0, 0);
    // carringtonLeftovers represents the extra .27 days between carrington rotations
    private TimeSpan carringtonLeftovers = new TimeSpan(0, 6, 29);
    private int counter = 0;

    void Update()
    {
        // counter is used so that images dont update every frame since long sim has more drastic changes
        if (counter%8==0)
        {
        // checks if it is first day in set of images, if yes then it sets now to start date
        if (index%136==0)
        {
            now = start;
            then = now;
            for (int i = 0; i < 27; i++)
            {
            then = then.Subtract(day);
            }
        }
        // if no, adds a carrington rotation time length
        else
        {
           for (int i = 0; i < 27; i++)
            {
            now = now.Add(day);
            }
            for (int i = 0; i < 27; i++)
            {
            then = then.Add(day);
            }
            now = now.Add(carringtonLeftovers);
            then = then.Add(carringtonLeftovers);
        }
        // updates date range in UI
        scrollbarText.text = "Date Range: \n" + then.ToString().Substring(0, then.ToString().IndexOf(' ')) + "-" + now.ToString().Substring(0, now.ToString().IndexOf(' '));
        scrollbar.value = ((float)(index%136))/136;
        index++;
        }
        counter++;
    }
}
