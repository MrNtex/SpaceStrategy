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
            switch (obj.bodyType)
            {
                case BodyType.Moon:
                    bodyIcon.sprite = SpriteManager.instance.moon;
                    break;
                default:
                    break;
            }
        }

        SetDescription(obj);
        SetStatus(obj.bodyStatus);

        panel.SetActive(true);
    }

    private void SetDescription(ObjectInfo body)
    {
        switch (body.bodyType)
        {
            case BodyType.Moon:
                bodyIcon.color = Color.gray;
                description.text = "Moon of " + body.orbiting.target.name;
                break;
            case BodyType.DwarfPlanet:
                bodyIcon.color = Color.gray;
                description.text = "Dwarf planet";
                break;
            case BodyType.Planet:
                switch (body.planetType)
                {
                    case PlanetType.Terrestrial:
                        description.text = "Terrestrial planet";
                        break;
                    case PlanetType.GasGiant:
                        description.text = "Gas giant";
                        break;
                    case PlanetType.IceGiant:
                        description.text = "Ice giant";
                        break;
                    default:
                        break;
                }
                break;
            case BodyType.Star:
                description.text = "Star";
                break;
            case BodyType.Main:
                description.text = "Main planet";
                break;
            default:
                bodyIcon.color = Color.white;
                break;
        }
    }

    private void SetStatus(BodyStatus body)
    {
        switch (body.status)
        {
            case BodyStatusType.Colonized:
                status.text = "Colonized";
                status.color = ColorManager.instance.colonized;
                break;
            case BodyStatusType.Colonizable:
                status.text = "Colonizable";
                status.color = ColorManager.instance.colonizable;
                break;
            case BodyStatusType.CanBeTerraformed:
                status.text = "Can be terraformed";
                status.color = ColorManager.instance.canBeTerraformed;
                break;
            case BodyStatusType.Specialized:
                status.text = "Specialized";
                status.color = ColorManager.instance.specialized;
                break;
            case BodyStatusType.CanBeSpecialized:
                status.text = "Can be specialized";
                status.color = ColorManager.instance.canBeSpecialized;
                break;
            case BodyStatusType.Inhabitable:
                status.text = "Inhabitable";
                status.color = ColorManager.instance.inhabitable;
                break;
            default:
                status.text = "";
                break;
        }
    }
}
