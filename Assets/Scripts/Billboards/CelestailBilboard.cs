using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelestailBilboard : Billboard
{
    [SerializeField]
    private List<Color> textColors;

    protected override void Start()
    {
        base.Start();
        SetupCelestailBody();
    }
    private void SetupCelestailBody()
    {
        BodyInfo bodyInfo = target.GetComponent<BodyInfo>();
        if (bodyInfo != null)
        {
            base.text.text = bodyInfo.bodyName;
        }
        PlanetFocusHelper planetFocusHelper = target.GetComponent<PlanetFocusHelper>();
        minDistance = planetFocusHelper.minDistance;

        button.GetComponent<Button>().onClick.AddListener(() => bodyInfo.ButtonClicked());

        text.color = textColors[(int)bodyInfo.bodyType];
    }
}
