using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyInfoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text bodyName;
    [SerializeField]
    private Image bodyIcon;
    public void SetBody(BodyInfo body)
    {
        if(body == null) // Used to hide the UI
        {
            panel.SetActive(false);
            return;
        }

        bodyName.text = body.bodyName;
        if(body.icon != null)
            bodyIcon.sprite = body.icon;
        panel.SetActive(true);
    }
}
