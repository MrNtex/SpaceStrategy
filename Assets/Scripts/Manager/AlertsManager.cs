using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AlertType
{
    Research,
    Decision
}
public class AlertsManager : MonoBehaviour
{
    public static AlertsManager Instance;

    [SerializeField]
    private List<Sprite> alertSprites = new List<Sprite>();

    public Color[] colors;

    [SerializeField]
    private GameObject[] alerts;

    public delegate void AlertTick(Color c);
    public AlertTick OnAlertTick;

    private void Awake()
    {
        Debug.Log("AlertsManager Awake");
        Instance = this;
    }
    public void ShowAlert(AlertType alert, TooltipData? td = null)
    {
        alerts[(int)alert].SetActive(true);

        alerts[(int)alert].GetComponent<AlertObject>().UpdateTooltipData(td);
    }
    public void HideAlert(AlertType alert)
    {
        alerts[(int)alert].SetActive(false);
        alerts[(int)alert].GetComponent<AlertObject>().UpdateTooltipData(null);
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

    private void Update()
    {
        Color animationColor = Color.Lerp(colors[0], colors[1], Mathf.Abs(Mathf.Sin(Time.time)));
        OnAlertTick?.Invoke(animationColor);
    }
}
