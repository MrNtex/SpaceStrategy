using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateManager : MonoBehaviour
{

    public DateTime currentDate;
    public DateTime startDate = new DateTime(2224, 4, 7);

    public static float timeScale = 1;

    [SerializeField]
    private TMP_Text dateText;
    private void Start()
    {
        currentDate = startDate;
    }
    public void UpdateDate(int elapsedDays)
    {
        currentDate = startDate.AddDays(elapsedDays);

        dateText.text = currentDate.ToString("dd-MM-yyyy");
    }
}
