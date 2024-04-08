using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInfo : MonoBehaviour
{
    public string bodyName;

    private PlanetFocusHelper planetFocusHelper;
    private CameraFocus cameraFocus;

    private void Awake()
    {
        if(bodyName == "") bodyName = gameObject.name;

        planetFocusHelper = GetComponent<PlanetFocusHelper>();
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
    }

    public void ButtonClicked()
    {
        cameraFocus.FocusOn(planetFocusHelper);
    }
}
