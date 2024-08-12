using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class date_script : MonoBehaviour
{
    // initiation
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI scrollbarText;
    private int index = 0;
    private DateTime start = new DateTime(2023, 7, 26);
    private DateTime now = new DateTime();
    private DateTime then = new DateTime();
    private TimeSpan day = new TimeSpan(24, 0, 0);
    void Update()
    {
        // checks if it is first day in set of images, if yes then it sets now to start date
        if (index%327==0)
        {
            now = start;
            then = now;
            for (int i = 0; i < 27; i++)
            {
            then = then.Subtract(day);
            }
        }
        // if no, adds a day
        else
        {
            now = now.Add(day);
            then = then.Add(day);
        }
        // updates date range in UI
        scrollbarText.text = "Date Range: \n" + then.ToString().Substring(0, then.ToString().IndexOf(' ')) + "-" + now.ToString().Substring(0, now.ToString().IndexOf(' '));
        scrollbar.value = ((float)(index%327))/327;
        index++;
    }
}
