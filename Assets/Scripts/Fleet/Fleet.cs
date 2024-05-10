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
public partial class Fleet : ObjectInfo
{
    [Header("Basic info")]
    public string fleetName;
    public List<Ship> composition = new List<Ship>();
    public GameObject capitan;
    [SerializeField]
    public GameObject destination;
    [HideInInspector]
    public float destinationOffset = 1;
    public FleetStatus status;

    [Header("Fleet stats")]
    
    public float speed = 15.0f;
    public float smoothTime = 0.3F; // Acceleration time

    public float maneuverabilityPenalty = 0.15f; // The multiplier of the angle modifier when ship has to rotate quickly, should be greater than 0
    public float minSpeed = 5.0f;

    public GameObject onOrbit;

    [Header("Formation")]
    public bool forceManeuver = false;

    
    public LineRenderer path;
    private const float lrOffset = 2.5f;

    
    private GameObject point;

    [SerializeField]
    private FleetBillboard fleetBillboard;

    public override void Start()
    {
        path = GetComponent<LineRenderer>();
        fleetBillboard.SetupFleet();

        UpdateFleet();

        point = new GameObject($"{fleetName}'s Point");
    }
    void UpdateFleet()
    {
        Debug.Log("Fleet " + fleetName + " has been updated, with capitan: " + capitan.name);

        FleetFormationHelper.instance.SetFormation(FleetFormation.Triangle, composition.ToArray(), capitan);
        fleetBillboard.UpdateFleet();
    }
    void RemoveFromFleet(Ship ship)
    {
        bool isCapitan = ship.prefab == capitan;
        Destroy(ship.prefab);
        composition.Remove(ship);
        if(isCapitan)
        {
            capitan = composition[0].prefab;
        }
        UpdateFleet();
    }
    

    public void SetDestination(GameObject dest)
    {
        if (dest.CompareTag("Point"))
        {
            point.transform.position = dest.transform.position;
            destination = point;
        }
        else
        {
            destination = dest;
        }

        if (dest.CompareTag("CelestialBody"))
        {
            destinationOffset = dest.transform.localScale.x * 2;
        }
        else
        {
            destinationOffset = .5f;
        }
        
        gameObject.transform.SetParent(null);

        SetFleetStatus(FleetStatus.Moving);

        // This is a temporary solution, i have to somehow reduce the calcuations to only x axis
        // It makes so that camera is looking at the middle point between the destination and the fleet

        Quaternion rotation = Quaternion.LookRotation(destination.transform.position - objectFocusHelper.cameraPlacement.position);
        Quaternion middlePoint = Quaternion.Slerp(rotation, objectFocusHelper.cameraPlacement.rotation, 0.5f);

        objectFocusHelper.cameraPlacement.rotation = Quaternion.Euler(middlePoint.eulerAngles.x, objectFocusHelper.cameraPlacement.rotation.eulerAngles.y, 0);
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
