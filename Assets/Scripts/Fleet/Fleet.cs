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
    public Ship[] composition;
    public GameObject capitan;
    public GameObject destination;
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

    private Vector3 velocity = Vector3.zero;
    

    private const float forceManueverDistance = 50f;
    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        path = GetComponent<LineRenderer>();

        Debug.Log("Fleet " + fleetName + " has been created, with capitan: " + capitan.name);
        Debug.Log(FleetFormationHelper.instance);
        FleetFormationHelper.instance.SetFormation(FleetFormation.Triangle, composition, capitan);
    }
    private void Update()
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
        if (Vector3.Distance(capitan.transform.position, dest) < .1f)
        {
            SetStatus(FleetStatus.Idle);
            return;
        }

        DrawPath(dest);

        Quaternion targetRotation = Quaternion.LookRotation(dest - capitan.transform.position);

        float distance = Vector3.Distance(capitan.transform.position, dest);

        Vector3 dir = dest - capitan.transform.position;
        float angle = Vector3.Angle(capitan.transform.forward, dir); // The resulting angle ranges from 0 to 180.

        float maneuverSpeed = speed;

        if (distance > forceManueverDistance && !forceManeuver)
        {
            //Speed up rotation when getting closer
            float angularMultiplier = .55f;
            if(distance < 400)
            {
                float ratio = 1-(distance - forceManueverDistance) / (400 - forceManueverDistance);
                angularMultiplier = Mathf.Lerp(.55f, 2, ratio);

                maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle * Mathf.Pow(ratio, 3), minSpeed, speed);
               // Debug.Log($"{ratio}, {maneuverSpeed}");
            }
            

            capitan.transform.rotation = Quaternion.SlerpUnclamped(capitan.transform.rotation, targetRotation, Time.deltaTime * angularMultiplier * DateManager.timeScale);
            capitan.transform.position = Vector3.SmoothDamp(capitan.transform.position, capitan.transform.position + capitan.transform.forward * 20, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);
            return;
        }


        maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle, minSpeed, speed);
        capitan.transform.position = Vector3.SmoothDamp(capitan.transform.position, dest, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);

        // Rotate much faster when close to destination
        capitan.transform.rotation = Quaternion.SlerpUnclamped(capitan.transform.rotation, targetRotation, Time.deltaTime * 2 * DateManager.timeScale);
    }
    public void SetDestination(GameObject dest)
    {
        destination = dest;

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
