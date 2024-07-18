using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    /// <summary>
    /// MUST BE AFTER DATE MANAGER IN THE EXECUTION ORDER
    /// </summary>
    public static WavesManager instance;

    public int currentWave = -1;

    public List<Wave> defaultWaves = new List<Wave>();
    public Queue<Wave> wavesQueue = new Queue<Wave>();
    public Wave nextWave;
    public int daysRemaining;

    public DateTime endDate;

    [SerializeField]
    private GameObject enemyPrefab;

    private const float spawnDistance = 4000;

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

        wavesQueue = new Queue<Wave>(defaultWaves);
    }

    void Start()
    {
        DateManager.instance.OnDateUpdate += HandleDateChanged;
        WaveFinished();
    }

    void HandleDateChanged()
    {
        daysRemaining = endDate.Subtract(DateManager.currentDate).Days;
        
        if(daysRemaining == 60 && !nextWave.firstWarningFired)
        {
            TooltipData waveIncomingData = new TooltipData("Wave aproaching", "", $"A wave of enemies is approaching in less than two months. They are aproaching from the {DirectionFromVector(nextWave.spawnPoint)}");
            AlertsManager.Instance.ShowAlert(AlertType.WaveIncoming, waveIncomingData);
            if (DateManager.instance.lastScale > 2)
            {
                DateManager.instance.UpdateTimeScale(2, true);
            }
            
            // Handle date changed can be called multiple times in a day, this is to prevent undestoyable alerts
            nextWave.firstWarningFired = true;
        }
        if(daysRemaining == 7 && !nextWave.secondWarningFired)
        {
            TooltipData waveIncomingData = new TooltipData("Wave aproaching", "", $"A wave of enemies is approaching in less than a week. They are aproaching from the {DirectionFromVector(nextWave.spawnPoint)}");
            AlertsManager.Instance.ShowAlert(AlertType.WaveClose, waveIncomingData);
            AlertsManager.Instance.HideAlert(AlertType.WaveIncoming);
            if (DateManager.instance.lastScale > 0)
            {
                DateManager.instance.UpdateTimeScale(0, true);
            }

            nextWave.secondWarningFired = true;
            return;
        }

        if(daysRemaining <= 0)
        {
            Debug.Log("Wave Incoming");
            EnemyFleet ef = Instantiate(enemyPrefab, nextWave.spawnPoint * spawnDistance, Quaternion.identity).GetComponent<EnemyFleet>();

            foreach(Ship ship in nextWave.composition)
            {
                ef.AddToFleet(ship, false);
            }

            WaveFinished();
        }
    }
    private string DirectionFromVector(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        if(angle < -90)
        {
            return "Betelgeuse";
        }
        if(angle < 0)
        {
            return "Vega";
        }
        if(angle < 90)
        {
            return "Arcturus";
        }
        return "Regulus";
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
            endDate = DateManager.currentDate;
            endDate = endDate.AddDays(10000);
            Debug.Log(endDate);
            return;
            // TODO: Implement infinite waves
        }
        endDate = DateManager.currentDate;
        endDate = endDate.AddDays(nextWave.delay);
    }
}

[System.Serializable]
public struct Wave
{
    public int delay;
    
    public List<Ship> composition;

    public Vector3 spawnPoint;

    public bool firstWarningFired, secondWarningFired;
}
