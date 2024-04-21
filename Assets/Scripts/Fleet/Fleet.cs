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

    public LineRenderer path;

    private FleetStatus status;
    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        path = GetComponent<LineRenderer>();
    }
    public void SetDestination(Vector3 dest)
    {
        destination = dest;

        capitan.GetComponent<FlyPattern>().target = dest;

        SetStatus(FleetStatus.Moving);
    }
    public void SetStatus(FleetStatus status)
    {
        this.status = status;
    }
    public override void ButtonClicked()
    {
        objectFocusHelper.cameraPlacement.SetParent(capitan.transform);
        BodyInfoUI.instance.SetBody(this);

        if (FleetManager.instance.selectedFleet == this)
        {
            cameraFocus.FocusOn(objectFocusHelper);
        }

        FleetManager.instance.SetSelectedFleet(this);
    }
    private void Update()
    {
        if (status == FleetStatus.Moving)
        {
            path.positionCount = 2;
            path.SetPosition(0, capitan.transform.position);
            path.SetPosition(1, destination);
        }
        else
        {
            path.positionCount = 0;
        }
    }
}
[System.Serializable]
public struct Ship
{
    public ShipType type;
    public int count;
}
