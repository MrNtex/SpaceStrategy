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
    [Header("Basic info")]
    public string fleetName;
    public List<Ship> composition = new List<Ship>();
    public GameObject capitan;
    [SerializeField]
    private GameObject destination;
    private float destinationOffset = 1;
    public FleetStatus status;

    [Header("Fleet stats")]
    
    public float speed = 15.0f;
    public float smoothTime = 0.3F; // Acceleration time

    public float maneuverabilityPenalty = 0.15f; // The multiplier of the angle modifier when ship has to rotate quickly, should be greater than 0
    public float minSpeed = 5.0f;

    [Header("Formation")]
    public bool forceManeuver = false;

    private CameraFocus cameraFocus;
    
    public LineRenderer path;
    private const float lrOffset = 2.5f;

    private Vector3 capitanVelocity = Vector3.zero;
    

    private const float forceManueverDistance = 50f;
    private GameObject point;

    [SerializeField]
    private FleetBillboard fleetBillboard;
    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
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
    private void FixedUpdate()
    {
        if (status == FleetStatus.Moving && destination != null)
        {
            if (destination.CompareTag("Point"))
            {
                FlyTowards(destination.transform.position);
                return;
            }

            Vector3 near = destination.transform.position - capitan.transform.position;
            near -= near.normalized;
            near += capitan.transform.position;
            FlyTowards(near);
            
        }
    }
    void FlyTowards(Vector3 dest)
    {
        if (Vector3.Distance(capitan.transform.position, dest) < destinationOffset)
        {
            if (destination.CompareTag("CelestialBody"))
            {
                gameObject.transform.SetParent(destination.transform);
            }
            SetStatus(FleetStatus.Idle);
            return;
        }

        DrawPath(dest);
        CalculateMovment(dest, capitan.transform, ref capitanVelocity);
    }

    public void CalculateMovment(Vector3 dest, Transform ship, ref Vector3 velocity)
    {
        Quaternion targetRotation = Quaternion.LookRotation(dest - ship.position);

        float distance = Vector3.Distance(ship.position, dest);

        Vector3 dir = dest - ship.position;
        float angle = Vector3.Angle(ship.forward, dir); // The resulting angle ranges from 0 to 180.

        float maneuverSpeed = speed;

        if (distance > forceManueverDistance && !forceManeuver)
        {
            //Speed up rotation when getting closer
            float angularMultiplier = .55f;
            if (distance < 400)
            {
                float ratio = 1 - (distance - forceManueverDistance) / (400 - forceManueverDistance);
                angularMultiplier = Mathf.Lerp(.55f, 2, ratio);

                maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle * Mathf.Pow(ratio, 3), minSpeed, speed);

                // Debug.Log($"{ratio}, {maneuverSpeed}");
            }


            ship.rotation = Quaternion.SlerpUnclamped(ship.rotation, targetRotation, Time.deltaTime * angularMultiplier * DateManager.timeScale);
            ship.position = Vector3.SmoothDamp(ship.position, ship.position + ship.forward * 20, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);
            return;
        }


        maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle, minSpeed, speed);
        ship.position = Vector3.SmoothDamp(ship.position, dest, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);

        // Rotate much faster when close to destination
        ship.rotation = Quaternion.SlerpUnclamped(ship.rotation, targetRotation, Time.deltaTime * 2 * DateManager.timeScale);
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
            destination = point;
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

        SetStatus(FleetStatus.Moving);
    }
    public void SetStatus(FleetStatus status)
    {
        this.status = status;

        switch (status)
        {
            case FleetStatus.Idle:
                DrawPath(capitan.transform.position); // Clear path
                break;
            default:
                Debug.LogError("Fleet " + fleetName + " has no status");
                break;
        }
        
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
