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
public class ObjectInfo : MonoBehaviour
{
    public string objectName;

    protected ObjectFocusHelper objectFocusHelper;
    private CameraFocus cameraFocus;
    public BodyStatus bodyStatus;

    [SerializeField]
    private bool useCustomColor = false;
    [SerializeField]
    private Color color;

    public Orbiting orbiting;

    public BodyType bodyType;
    public PlanetType planetType;

    public Sprite icon;

    
    private void Awake()
    {
        if(objectName == "") objectName = gameObject.name;

        objectFocusHelper = GetComponent<ObjectFocusHelper>();
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        
    }
    
    private void Start()
    {
        if (useCustomColor)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        bodyStatus = GetComponent<BodyStatus>();
        orbiting = GetComponent<Orbiting>();
    }
    public void ButtonClicked()
    {
        cameraFocus.FocusOn(objectFocusHelper);
    }
}
