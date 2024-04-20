using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelestailBilboard : Billboard
{
    

    protected override void Start()
    {
        base.Start();
        SetupCelestailBody();
    }
    private void SetupCelestailBody()
    {
        BodyInfo bodyInfo = target.GetComponent<BodyInfo>();
        Debug.Log(bodyInfo);
        if (bodyInfo != null)
        {
            base.text.text = bodyInfo.objectName;
        }
        ObjectFocusHelper planetFocusHelper = target.GetComponent<ObjectFocusHelper>();
        minDistance = planetFocusHelper.minDistance;

        button.GetComponent<Button>().onClick.AddListener(() => bodyInfo.ButtonClicked());

        text.color = bodyInfo.GetColor();
    }
}
