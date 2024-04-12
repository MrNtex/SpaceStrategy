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
public enum PlanetType
{
    Terrestrial,
    GasGiant,
    IceGiant
}
public class BodyInfo : MonoBehaviour
{
    public string bodyName;

    private PlanetFocusHelper planetFocusHelper;
    private CameraFocus cameraFocus;
    public Orbiting orbiting;

    [SerializeField]
    private bool useCustomColor = false;
    [SerializeField]
    private Color color;

    public BodyType bodyType;
    public PlanetType planetType;

    public Sprite icon;
    private void Awake()
    {
        if(bodyName == "") bodyName = gameObject.name;

        planetFocusHelper = GetComponent<PlanetFocusHelper>();
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        orbiting = GetComponent<Orbiting>();
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
