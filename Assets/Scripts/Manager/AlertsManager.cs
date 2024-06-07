using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum AlertType
{
    Research,
    Decision
}
public class AlertsManager : MonoBehaviour
{
    public static AlertsManager Instance;

    public Color[] colors;

    [SerializeField]
    private GameObject[] alerts;

    private List<Image> activeAlerts = new List<Image>();
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
    public void ShowAlert(AlertType alert, TooltipData? td = null)
    {
        alerts[(int)alert].SetActive(true);

        alerts[(int)alert].GetComponent<AlertObject>().UpdateTooltipData(td);

        activeAlerts.Add(alerts[(int)alert].GetComponent<Image>());
    }
    public void HideAlert(AlertType alert)
    {
        alerts[(int)alert].SetActive(false);
        alerts[(int)alert].GetComponent<AlertObject>().UpdateTooltipData(null);

        activeAlerts.Remove(alerts[(int)alert].GetComponent<Image>());
    }
    public void OnAlertClick(AlertType alertType)
    {
        switch (alertType)
        {
            case AlertType.Research:
                Debug.Log("Research Alert Clicked");
                break;
            case AlertType.Decision:
                Debug.Log("Decision Alert Clicked");
                break;
        }
    }

    
}
