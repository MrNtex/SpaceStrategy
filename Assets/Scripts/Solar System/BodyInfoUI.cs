using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BodyInfoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text bodyName, description, status;
    [SerializeField]
    private Image bodyIcon;

    [SerializeField]
    private Color gray;
    public void SetBody(ObjectInfo obj)
    {
        if (obj == null) // Used to hide the UI
        {
            panel.SetActive(false);
            return;
        }

        bodyName.text = obj.objectName;
        if (obj.icon != null)
        {
            bodyIcon.sprite = obj.icon;
        }
        else
        {
            
        }

        description.text = obj.GetDescrition();
        obj.SetStatus(ref status);

        panel.SetActive(true);
    }


    
}
