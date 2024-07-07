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
    private Button specialButton;
    private Image specialButtonImage;

    [SerializeField]
    private TMP_Text bodyName, description, status;
    [SerializeField]
    private Image bodyIcon;

    [SerializeField]
    private Color gray;

    private ObjectInfo currentBody;

    [SerializeField]
    private Sprite square, hexagon, pentagon;
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

        specialButtonImage = specialButton.GetComponent<Image>();
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

        Sprite shape = GetShape(obj);

        if(shape != null)
        {
            specialButtonImage.sprite = shape;
            specialButton.interactable = true;

            specialButton.onClick.RemoveAllListeners();
            specialButton.onClick.AddListener(() => obj.SpecialButtonClicked());
            specialButton.onClick.AddListener(() => SetBody(null)); // Hide the UI
        }
        else
        {
            specialButtonImage.sprite = null;
            specialButton.interactable = false;
        }

        panel.SetActive(true);
    }
    public Sprite GetShape(ObjectInfo obj)
    {
        if(obj is BodyInfo)
        {
            switch (((BodyInfo)obj).status)
            {
                case BodyStatusType.CanBeSpecialized:
                case BodyStatusType.Specialized:
                case BodyStatusType.CanBeTerraformed:
                    return pentagon;
                case BodyStatusType.Colonizable:
                case BodyStatusType.Colonized:
                    return hexagon;
                default:
                    return null;
            }
        }
        if(obj is Fleet)
        {
            if(obj is FriendlyFleet)
            {
                return square;
            }
            return hexagon;
        }
        return null;
    }
}
