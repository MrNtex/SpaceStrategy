using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
    /// <summary>
    /// Date updates are handled by the object with orbiting script connected to the date manager
    /// The object is saved as the DateObject
    /// </summary>
    /// 
    public static DateManager instance;

    public static DateTime currentDate;
    public DateTime startDate = new DateTime(2224, 4, 7);

    public static float timeScale = 1;

    [SerializeField]
    private TMP_Text dateText;

    [SerializeField]
    private GameObject[] scaleStamps;

    [SerializeField]
    private Color active, inactive, maxActive;


    private readonly float[] timeScales = { 1, 5, 10, 15, 50};
    public int lastScale = 1;

    public static GameObject DateObject;

    public delegate void DateUpdate();
    public event DateUpdate OnDateUpdate;

    public delegate void MonthUpdate();
    public event MonthUpdate OnMonthUpdate;

    int lastMonth = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple DateManagers in scene");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentDate = startDate;
        UpdateTimeScale(0);
    }
    public void UpdateDate(float days)
    {
        currentDate = currentDate.AddDays(days);

        dateText.text = currentDate.ToString("dd-MM-yyyy");

        OnDateUpdate?.Invoke();

        if (currentDate.Day == 1 && lastMonth != currentDate.Month)
        {
            lastMonth = currentDate.Month;
            OnMonthUpdate?.Invoke();
        }
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
    public void UpdateTimeScale(int idx, bool indirectly)
    {
        // It's a bit of a hack, but it's a way to prevent the time scale from being changed by events, without having to remake the button system

        // Indirectly is left to create a setting that will disable the time scale change by events

        UpdateTimeScale(idx);
    }
    public void Pause(bool force = false)
    {
        if (!force && timeScale == 0)
        {
            Resume();
            return;
        }
        timeScale = 0;
    }
    public void Resume()
    {
        UpdateTimeScale(lastScale);
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
