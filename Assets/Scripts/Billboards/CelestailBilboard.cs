using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CelestailBilboard : Billboard
{
    [SerializeField]
    private Sprite pentagon, hexagon;
    [SerializeField]
    private Color canSpecializedColor, specializedColor, canBeColonizedColor, colonizedColor, canBeTerraformedColor;
    [SerializeField]
    private GameObject specialButton;

    private HorizontalLayoutGroup layoutGroup;

    [SerializeField]
    private int offset = 15;

    private Image specialButtonImg;
    private BodyInfo bodyInfo;
    protected override void Start()
    {
        base.Start();
        SetupCelestailBody();
    }
    private void SetupCelestailBody()
    {
        bodyInfo = target.GetComponent<BodyInfo>();

        if (bodyInfo != null)
        {
            base.text.text = bodyInfo.objectName;
        }
        ObjectFocusHelper planetFocusHelper = target.GetComponent<ObjectFocusHelper>();
        minDistance = planetFocusHelper.minDistance;

        button.GetComponent<Button>().onClick.AddListener(() => bodyInfo.ButtonClicked());
        specialButton.GetComponent<Button>().onClick.AddListener(() => bodyInfo.SpecialButtonClicked());

        layoutGroup = button.GetComponent<HorizontalLayoutGroup>();

        specialButtonImg = specialButton.GetComponent<Image>();

        text.color = bodyInfo.GetColor();

        UpdateBillboard();
    }

    void UpdateBillboard()
    {
        bodyInfo = target.GetComponent<BodyInfo>();
        switch (bodyInfo.bodyStatus.status)
        {
            case BodyStatusType.Inhabitable:
                layoutGroup.padding.left = 0;
                specialButton.SetActive(false);
                break;
            case BodyStatusType.CanBeSpecialized:
                specialButton.SetActive(true);
                specialButtonImg.sprite = pentagon;
                specialButtonImg.color = canSpecializedColor;
                layoutGroup.padding.left = offset;
                break;
            case BodyStatusType.Specialized:
                specialButton.SetActive(true);
                specialButtonImg.sprite = pentagon;
                specialButtonImg.color = specializedColor;
                layoutGroup.padding.left = offset;
                break;
            case BodyStatusType.CanBeTerraformed:
                specialButton.SetActive(true);
                specialButtonImg.sprite = hexagon;
                specialButtonImg.color = canBeTerraformedColor;
                layoutGroup.padding.left = offset;
                break;
            case BodyStatusType.Colonizable:
                specialButton.SetActive(true);
                specialButtonImg.sprite = hexagon;
                specialButtonImg.color = canBeColonizedColor;
                layoutGroup.padding.left = offset;
                break;
            case BodyStatusType.Colonized:
                specialButton.SetActive(true);
                specialButtonImg.sprite = hexagon;
                specialButtonImg.color = colonizedColor;
                layoutGroup.padding.left = offset;
                break;
            default:
                layoutGroup.padding.left = offset;
                specialButton.SetActive(false);
                break;
        }
    }
    public void RightClick()
    {
        mainCamera.GetComponent<CameraRightClick>().onRightClick(target.gameObject); 
    }
}
