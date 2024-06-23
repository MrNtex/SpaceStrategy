using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum AlertType
{
    Research,
    Decision,
    WaveClose,
    WaveIncoming
}
public class AlertsManager : MonoBehaviour
{
    public static AlertsManager Instance;

    public Color[] colors;

    [SerializeField]
    private GameObject[] alerts;

    private List<Image> activeAlerts = new List<Image>();

    [SerializeField]
    private Tooltip tooltip;
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

        activeAlerts.Remove(alerts[(int)alert].GetComponent<Image>());;

        if (tooltip != null && tooltip.gameObject.activeSelf && tooltip.target == TooltipTarget.Alert)
        {
            
            tooltip.HideTooltip();
        }
    }
    public void OnAlertClick(AlertType alertType)
    {
        switch (alertType)
        {
            case AlertType.Research:
                MenusManager.Instance.ChangeMenu(1);
                break;
            case AlertType.Decision:
                MenusManager.Instance.ChangeMenu(2);
                break;
        }
    }

    
}
