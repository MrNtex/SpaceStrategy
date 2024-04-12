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
    private TMP_Text bodyName, description;
    [SerializeField]
    private Image bodyIcon;

    [SerializeField]
    private Color gray;
    public void SetBody(BodyInfo body)
    {
        if (body == null) // Used to hide the UI
        {
            panel.SetActive(false);
            return;
        }

        bodyName.text = body.bodyName;
        if (body.icon != null)
        {
            bodyIcon.sprite = body.icon;
        }
        else
        {
            switch (body.bodyType)
            {
                case BodyType.Moon:
                    bodyIcon.sprite = SpriteManager.instance.moon;
                    break;
                default:
                    break;
            }
        }

        SetDescription(body);

        panel.SetActive(true);
    }

    private void SetDescription(BodyInfo body)
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
}
