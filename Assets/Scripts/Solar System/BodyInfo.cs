using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyType
{
    Planet,
    Main, // The planet that determines the time scale (Earth)
    Star,
    Moon,
    DwarfPlanet
}
public class BodyInfo : MonoBehaviour
{
    public string bodyName;

    private PlanetFocusHelper planetFocusHelper;
    private CameraFocus cameraFocus;

    [SerializeField]
    private bool useCustomColor = false;
    [SerializeField]
    private Color color;

    public BodyType bodyType;

    public Sprite icon;
    private void Awake()
    {
        if(bodyName == "") bodyName = gameObject.name;

        planetFocusHelper = GetComponent<PlanetFocusHelper>();
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
    }
    private void Start()
    {
        if(useCustomColor)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
    public void ButtonClicked()
    {
        cameraFocus.FocusOn(planetFocusHelper);
    }
}
