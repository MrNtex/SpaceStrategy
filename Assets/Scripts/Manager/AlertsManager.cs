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
    [SerializeField]
    private GameObject[] alerts;

    public static AlertsManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowAlert(AlertType alertType)
    {
        alerts[(int)alertType].SetActive(true);
    }
    public void HideAlert(AlertType alertType)
    {
        alerts[(int)alertType].SetActive(false);
    }
}
