using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum AlertType
{
    Research,
    Decision,
    WaveClose,
    WaveIncoming,
    Colony
}
public class AlertsManager : MonoBehaviour
{
    public static AlertsManager Instance;


    [SerializeField]
    private GameObject alertIcon;

    private Dictionary<AlertData, GameObject> activeAlerts = new Dictionary<AlertData, GameObject>();

    [SerializeField]
    private Tooltip tooltip;

    [SerializeField]
    private Transform alertsBar;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowAlert(AlertData alert)
    {
        if(activeAlerts.ContainsKey(alert))
        {
            return;
        }

        AlertObject alertObject = Instantiate(alertIcon, alertsBar).GetComponent<AlertObject>();

        alertObject.SetUp(alert, tooltip);

        activeAlerts.Add(alert, alertObject.gameObject);
    }
    public void HideAlert(AlertData alert)
    {
        if(!activeAlerts.ContainsKey(alert))
        {
            return;
        }

        Destroy(activeAlerts[alert]);

        activeAlerts.Remove(alert);

        if (tooltip != null && tooltip.gameObject.activeSelf && tooltip.target == TooltipTarget.Alert)
        {
            
            tooltip.HideTooltip();
        }
    }
}
public class AlertData
{
    public AlertType alertType;
    
    public TooltipData tooltipData;

    public bool severe;

    public Action onClick;

    public Sprite icon;

    public AlertData(AlertType alertType, TooltipData tooltipData, bool severe, Action onClick, Sprite icon)
    {
        this.alertType = alertType;
        this.tooltipData = tooltipData;
        this.severe = severe;
        this.onClick = onClick;
        this.icon = icon;
    }
}