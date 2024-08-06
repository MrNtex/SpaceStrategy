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

    [SerializeField]
    private GameObject leftPanel;
    [SerializeField]
    private GameObject leftPanelEffectPrefab;
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
            leftPanel.SetActive(false);
            return;
        }


        
        if (obj.effects.Count > 0)
        {
            CreateLeftPanel(obj);
        }
        else
        {
            leftPanel.SetActive(false);
        }

        bodyName.text = obj.objectName;
        if (obj.icon != null)
        {
            bodyIcon.sprite = obj.icon;
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

    public void CreateLeftPanel(ObjectInfo obj)
    {
        for (int i = leftPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(leftPanel.transform.GetChild(i).gameObject);
        }

        leftPanel.SetActive(true);

        foreach (LeftPanelEffect effect in obj.effects)
        {
            GameObject effectObj = Instantiate(leftPanelEffectPrefab, leftPanel.transform);
            effectObj.GetComponent<LeftPanelEffectUI>().Create(effect);
        }
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
