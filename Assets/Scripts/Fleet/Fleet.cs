using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShipType
{
    Fighter,
    Destroyer,
    Cruiser,
    Battleship,
    Dreadnought
}
public class Fleet : ObjectInfo
{
    public GameObject capitan;
    public GameObject[] ships;

    public string fleetName;

    public Ship[] composition;

    private CameraFocus cameraFocus;
    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
    }
    public void Focus()
    {
        objectFocusHelper.cameraPlacement.SetParent(capitan.transform);
        cameraFocus.FocusOn(objectFocusHelper);
    }
}
[System.Serializable]
public struct Ship
{
    public ShipType type;
    public int count;
}
