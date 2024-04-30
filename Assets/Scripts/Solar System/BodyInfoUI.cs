using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BodyInfoUI : MonoBehaviour
{
    public static BodyInfoUI instance;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text bodyName, description, status;
    [SerializeField]
    private Image bodyIcon;

    [SerializeField]
    private Color gray;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
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

        description.text = obj.GetDescription();
        obj.SetStatus(ref status);

        panel.SetActive(true);
    }


    
}
