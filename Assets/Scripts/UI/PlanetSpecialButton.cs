using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSpecialButton : MonoBehaviour
{
    [SerializeField]
    private Image background, image, outline;

    [SerializeField]
    private Sprite hexagon, hexagonOutline, pentagon, pentagonOutline;

    [SerializeField]
    private Color bgColor;

    public void SetUp(BodyStatusType type, Sprite icon = null)
    {

        background.color = bgColor;
        switch (type)
        {
            case BodyStatusType.Inhabitable:
                break;
            case BodyStatusType.CanBeSpecialized:
                background.sprite = pentagon;
                outline.sprite = pentagonOutline;
                outline.color = ColorManager.instance.canBeSpecialized;
                break;
            case BodyStatusType.Specialized:
                background.sprite = pentagon;
                outline.sprite = pentagonOutline;
                outline.color = ColorManager.instance.specialized;
                break;
            case BodyStatusType.CanBeTerraformed:
                background.sprite = hexagon;
                outline.sprite = hexagonOutline;
                outline.color = ColorManager.instance.canBeTerraformed;
                break;
            case BodyStatusType.Colonizable:
                background.sprite = hexagon;
                outline.sprite = hexagonOutline;
                outline.color = ColorManager.instance.colonizable;
                break;
            case BodyStatusType.Colonized:
                background.sprite = hexagon;
                outline.sprite = hexagonOutline;
                outline.color = ColorManager.instance.colonized;
                break;
            default:
                break;
        }

        if (icon == null)
        {
            image.gameObject.SetActive(false);
            return;
        }
        image.gameObject.SetActive(true);
        image.sprite = icon;
    }
}
