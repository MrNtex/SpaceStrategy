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

    private Vector3 destination;
    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
    }
    public void Focus()
    {
        objectFocusHelper.cameraPlacement.SetParent(capitan.transform);
        cameraFocus.FocusOn(objectFocusHelper);

        FleetManager.instance.selectedFleet = this;
    }
    public void SetDestination(Vector3 dest)
    {
        destination = dest;

        capitan.GetComponent<FlyPattern>().target = dest;
    }
}
[System.Serializable]
public struct Ship
{
    public ShipType type;
    public int count;
}
