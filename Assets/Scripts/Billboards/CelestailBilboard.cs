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


    private HorizontalLayoutGroup layoutGroup;

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
            layoutGroup.padding.left = specialButtonTextOffset;
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
