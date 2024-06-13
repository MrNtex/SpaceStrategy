using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum ShipType
{
    Fighter,
    Destroyer,
    Cruiser,
    Battleship,
    Dreadnought
}
public partial class FriendlyFleet : Fleet
{
    [Header("Formation")]
    public bool forceManeuver = false;

    
    public LineRenderer path;
    private const float lrOffset = 2.5f;


    public void Start()
    {
        path = GetComponent<LineRenderer>();
        fleetBillboard.SetupFleet();

        base.UpdateFleet();

        point = new GameObject($"{objectName}'s Point");
    }

    public override void SetDestination(GameObject dest)
    {
        base.SetDestination(dest);
        SetFleetStatus(FleetStatus.Moving);
    }

    public void SetFleetStatus(FleetStatus status)
    {
        this.status = status;

        if(status != FleetStatus.Idle) onOrbit = null;

        switch (status)
        {
            case FleetStatus.Idle:
                DrawPath(capitan.transform.position); // Clear path
                break;
            default:
                break;
        }

        BodyInfoUI.instance.SetBody(this, false);
    }
    public override void ButtonClicked()
    {
        objectFocusHelper.cameraPlacement.SetParent(capitan.transform);
        BodyInfoUI.instance.SetBody(this);

        cameraFocus.FocusOn(objectFocusHelper, FleetManager.instance.selectedFleet == this);

        FleetManager.instance.selectedFleet = this; // Reminder: Selected Object and Selected Fleet must be different (they are changed under similar yet diffrent circumstances)
    }
    public void DrawPath(Vector3 dest)
    {
        if (status == FleetStatus.Moving)
        {
            path.positionCount = 2;
            path.SetPosition(0, capitan.transform.position + capitan.transform.forward * lrOffset);
            path.SetPosition(1, dest);
        }
        else
        {
            path.positionCount = 0;
        }
    }

    public override void SetStatus(ref TMP_Text text)
    {
        text.color = ColorManager.instance.fleetDestination;
        switch (status)
        {
            case FleetStatus.Idle:
                if (onOrbit)
                {
                    text.text = $"Orbiting <link=\"CelestialBody\"><color=#ffd666>{onOrbit.name}</color></link>";
                    break;
                }
                text.text = "Idle";
                break;
            case FleetStatus.Moving:
                if(destination.CompareTag("CelestialBody"))
                {
                    text.text = $"Moving to <link=\"CelestialBody\"><color=#ffd666>{destination.name}</color></link>";
                    ClickableLinkHandler.adress = destination;
                }
                else
                {
                    string point = "X: " + destination.transform.position.x + " Y: " + destination.transform.position.z; // Y because of the 2D plane
                    text.text = "Moving to " + point;
                }
                break;
            default:
                text.text = "No status";
                break;
        }
    }
}
[System.Serializable]
public struct Ship
{
    public ShipType type;
    //public int count;
    public FlyPattern flyPattern;
    public GameObject prefab;

    void Start()
    {
        if(prefab == null)
        {
            Debug.LogError("Prefab is not set for " + type);
        }
        flyPattern = prefab.GetComponent<FlyPattern>();
        if(flyPattern == null)
        {
            Debug.LogError("FlyPattern is not set for " + type);
        }
    }
}
