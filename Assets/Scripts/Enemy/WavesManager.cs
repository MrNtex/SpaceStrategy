using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    /// <summary>
    /// MUST BE AFTER DATE MANAGER IN THE EXECUTION ORDER
    /// 
    /// 
    /// </summary>
    public static WavesManager instance;

    public int currentWave = -1;

    public List<Wave> defaultWaves = new List<Wave>();
    public Wave nextWave;
    public int daysRemaining;

    public DateTime endDate;

    private TooltipData waveCloseData = new TooltipData("Wave Incoming", "", "A wave of enemies is approaching in less than a month. Prepare your defenses.");
    private TooltipData waveIncomingData = new TooltipData("Wave Incoming", "", "A wave of enemies is approaching in less than a week. Prepare your defenses.");
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple WavesManagers in scene");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DateManager.instance.OnDateUpdate += HandleDateChanged;

    }

    void HandleDateChanged()
    {
        daysRemaining = endDate.Subtract(DateManager.currentDate).Days;
        
        if(daysRemaining == 30)
        {
            AlertsManager.Instance.ShowAlert(AlertType.WaveIncoming, waveCloseData);
            return;
        }
        if(daysRemaining == 7)
        {
            AlertsManager.Instance.ShowAlert(AlertType.WaveClose, waveIncomingData);
            AlertsManager.Instance.HideAlert(AlertType.WaveIncoming);
            return;
        }

        if(daysRemaining <= 0)
        {
            Debug.Log("Wave Incoming");
        }
    }

    void WaveFinished()
    {
        currentWave++;

        if(currentWave < defaultWaves.Count)
        {
            nextWave = defaultWaves[currentWave];
        }
        else
        {
            // TODO: Implement infinite waves
        }
        endDate = DateManager.currentDate.AddDays(nextWave.delay);
    }
}

[System.Serializable]
public struct Wave
{
    public int delay;
    
    public int power;
}
