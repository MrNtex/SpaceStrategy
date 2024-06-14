using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public partial class Fleet : ObjectInfo
{
    [Header("Basic info")]
    public List<Ship> composition = new List<Ship>();
    public GameObject capitan
    {
        set
        {
            capitanIndex = composition.FindIndex(x => x.prefab == value);
        }
        get
        {
            return composition[capitanIndex].prefab;
        }
    }
    [SerializeField]
    private int capitanIndex = 0;
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

    [SerializeField]
    protected FleetBillboard fleetBillboard;

    public GameObject onOrbit;

    protected GameObject point;


    public LineRenderer path;
    protected const float lrOffset = 2.5f;

    private Transform mainCamera;

    void Start()
    {
        path = GetComponent<LineRenderer>();

        fleetBillboard.SetUpFleet();
        UpdateFleet(false);

        point = new GameObject($"{objectName}'s Point");

        mainCamera = Camera.main.transform;
    }

    public virtual void SetDestination(GameObject dest)
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

        // This is a temporary solution, i have to somehow reduce the calcuations to only x axis
        // It makes so that camera is looking at the middle point between the destination and the fleet

        Quaternion rotation = Quaternion.LookRotation(destination.transform.position - objectFocusHelper.cameraPlacement.position);
        Quaternion middlePoint = Quaternion.Slerp(rotation, objectFocusHelper.cameraPlacement.rotation, 0.5f);

        objectFocusHelper.cameraPlacement.rotation = Quaternion.Euler(middlePoint.eulerAngles.x, objectFocusHelper.cameraPlacement.rotation.eulerAngles.y, 0);
    }
    public void UpdateFleet(bool billboard = true)
    {
        Debug.Log("Fleet " + objectName + " has been updated, with capitan: " + capitan.name);

        FleetFormationHelper.instance.SetFormation(FleetFormation.Triangle, composition.ToArray(), capitan);
        if(billboard) fleetBillboard.UpdateFleet();
    }

    void RemoveFromFleet(Ship ship)
    {
        bool isCapitan = ship.prefab == capitan;
        Destroy(ship.prefab);
        composition.Remove(ship);
        if (isCapitan)
        {
            capitan = composition[0].prefab;
        }
        UpdateFleet();
    }

    public virtual void DrawPath(Vector3 dest) { }
    public virtual void SetFleetStatus(FleetStatus status) { }
}
[System.Serializable]
public class Ship
{
    public ShipType type;
    //public int count;
    public Vector3 velocity;
    public GameObject prefab;
    public Vector3 myOffset;
}

public enum FleetFlyPatternType
{
    Follow,
    Orbit,
    Attack,
    Flee
}
public enum FleetFormation
{
    Line,
    Circle,
    Triangle,
    Square
}
public enum FleetStatus
{
    Idle,
    Moving,
    OnOrbit,
    Attacking,
    Fleeing
}
