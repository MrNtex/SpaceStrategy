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


    private CameraFocus cameraFocus;
    
    public LineRenderer path;
    private const float lrOffset = 2.5f;

    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;
    public float speed = 15.0f;

    void Start()
    {
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        path = GetComponent<LineRenderer>();
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
        capitan.transform.rotation = Quaternion.SlerpUnclamped(capitan.transform.rotation, Quaternion.LookRotation(dest - capitan.transform.position), Time.deltaTime * .55f * DateManager.timeScale);

        if (Vector3.Distance(capitan.transform.position, dest) > 30f)
        {
            capitan.transform.position = Vector3.SmoothDamp(capitan.transform.position, capitan.transform.position + capitan.transform.forward * 20, ref velocity, smoothTime, speed, Time.deltaTime * DateManager.timeScale);
            return;
        }
        capitan.transform.position = Vector3.SmoothDamp(capitan.transform.position, dest, ref velocity, smoothTime, speed, Time.deltaTime * DateManager.timeScale);

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
    public int count;
}
