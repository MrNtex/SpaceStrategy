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
        ObjectInfo bodyInfo = target.GetComponent<ObjectInfo>();
        if (bodyInfo != null)
        {
            base.text.text = bodyInfo.objectName;
        }
        ObjectFocusHelper planetFocusHelper = target.GetComponent<ObjectFocusHelper>();
        minDistance = planetFocusHelper.minDistance;

        button.GetComponent<Button>().onClick.AddListener(() => bodyInfo.ButtonClicked());

        text.color = textColors[(int)bodyInfo.bodyType];
    }
}
