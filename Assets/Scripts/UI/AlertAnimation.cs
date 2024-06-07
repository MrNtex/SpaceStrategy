using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertAnimation : MonoBehaviour
{
    // MUST BE AFTER ALERTS MANAGER IN THE EXECUTION ORDER !!!
    //
    // OnEnable sometimes is called before Awake in AlertsManager

    [SerializeField]
    private Image bg;

    private void OnEnable()
    {
        Debug.Log("AlertAnimation OnEnable");
        AlertsManager.Instance.OnAlertTick += Animate;
    }
    private void OnDisable()
    {
        AlertsManager.Instance.OnAlertTick -= Animate;
    }

    private void Animate(Color c)
    {
        bg.color = c;
    }
}
