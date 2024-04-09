using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{

    public DateTime currentDate;
    public DateTime startDate = new DateTime(2224, 4, 7);

    public static float timeScale = 1;

    [SerializeField]
    private TMP_Text dateText;

    [SerializeField]
    private GameObject[] scaleStamps;

    [SerializeField]
    private Color active, inactive, maxActive;

    private const float maxTimeScale = 10;
    private void Start()
    {
        currentDate = startDate;
    }
    public void UpdateDate(int elapsedDays)
    {
        currentDate = startDate.AddDays(elapsedDays);

        dateText.text = currentDate.ToString("dd-MM-yyyy");
    }
    public void UpdateTimeScale(float timeScale)
    {
        DateManager.timeScale = timeScale;
        int i = 0;
        for (; i < scaleStamps.Length-1 && i < Mathf.Ceil(DateManager.timeScale); i++)
        {
            scaleStamps[i].GetComponent<Image>().color = active;
        }
        for(; i < scaleStamps.Length - 1; i++)
        {
            scaleStamps[i].GetComponent<Image>().color = inactive;
        }
        if(DateManager.timeScale >= maxTimeScale)
        {
            scaleStamps[scaleStamps.Length - 1].GetComponent<Image>().color = maxActive;
        }else
        {
            scaleStamps[scaleStamps.Length - 1].GetComponent<Image>().color = inactive;
        }
    }
}
