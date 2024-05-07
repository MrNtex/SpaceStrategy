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

    private ObjectInfo currentBody;
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
    /// <summary>
    /// Sets UI to display the information of the object
    /// Force is used to determine whether its just a refresh or a new object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="force"></param>
    public void SetBody(ObjectInfo obj, bool force = true)
    {
        if(!force && obj != currentBody)
        {
            return;
        }

        currentBody = obj;

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
