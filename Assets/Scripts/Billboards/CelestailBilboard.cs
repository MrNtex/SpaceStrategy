using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CelestailBilboard : Billboard
{
    // MUST BE AFTER ALERTS MANAGER IN THE EXECUTION ORDER !!!

    [SerializeField]
    private Sprite pentagon, hexagon;
    [SerializeField]
    private Color canSpecializedColor, specializedColor, canBeColonizedColor, colonizedColor, canBeTerraformedColor;
    [SerializeField]
    private GameObject specialButton;
    private PlanetSpecialButton planetSpecialButton;

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

        specialButton.GetComponent<Button>().onClick.AddListener(() => bodyInfo.SpecialButtonClicked());
        specialButtonImg = specialButton.GetComponent<Image>();
        planetSpecialButton = specialButton.GetComponent<PlanetSpecialButton>();

        button.GetComponent<Button>().onClick.AddListener(() => bodyInfo.ButtonClicked());
        layoutGroup = button.GetComponent<HorizontalLayoutGroup>();

        text.color = bodyInfo.GetColor();

        UpdateBillboard();
    }

    void UpdateBillboard()
    {
        bodyInfo = target.GetComponent<BodyInfo>();

        if(bodyInfo != null && bodyInfo.status != BodyStatusType.Inhabitable)
        {
            specialButton.SetActive(true);
            planetSpecialButton.SetUp(bodyInfo.status, bodyInfo.icon);
            layoutGroup.padding.left = offset;
            return;
        }

        layoutGroup.padding.left = 0;
        specialButton.SetActive(false);
    }
    public void RightClick()
    {
        mainCamera.GetComponent<CameraRightClick>().onRightClick(target.gameObject); 
    }
}
