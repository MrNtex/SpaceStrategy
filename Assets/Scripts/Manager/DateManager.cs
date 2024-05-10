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


    private readonly float[] timeScales = { 1, 5, 10, 20, 100};
    int lastScale = 1;
    private void Start()
    {
        currentDate = startDate;
        UpdateTimeScale(0);
    }
    public void UpdateDate(float days)
    {
        currentDate = currentDate.AddDays(days);

        dateText.text = currentDate.ToString("dd-MM-yyyy");
    }
    public void UpdateTimeScale(int idx)
    {
        DateManager.timeScale = timeScales[idx];
        int i = 0;
        for (; i <= idx; i++)
        {
            scaleStamps[i].GetComponent<Image>().color = active;
        }
        for(; i < scaleStamps.Length - 1; i++)
        {
            scaleStamps[i].GetComponent<Image>().color = inactive;
        }
        if(idx == scaleStamps.Length - 1)
        {
            scaleStamps[scaleStamps.Length - 1].GetComponent<Image>().color = maxActive;
        }else
        {
            scaleStamps[scaleStamps.Length - 1].GetComponent<Image>().color = inactive;
        }

        lastScale = idx;
    }
    public void Pause()
    {
        if (timeScale == 0)
        {
            UpdateTimeScale(lastScale);
            return;
        }
        timeScale = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UpdateTimeScale(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) UpdateTimeScale(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) UpdateTimeScale(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) UpdateTimeScale(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) UpdateTimeScale(4);
        else if (Input.GetKeyDown(KeyCode.Space)) Pause();
    }

}
