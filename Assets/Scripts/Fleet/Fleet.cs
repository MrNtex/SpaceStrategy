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

    public GameObject[] ships;

    public string fleetName;

    public Ship[] composition;

    private CameraFocus cameraFocus;

    public GameObject destination;


    public LineRenderer path;

    public FleetStatus status;


    // Capitan
    public GameObject capitan;

    private Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.3F;

    public float speed = 5.0f;
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
        capitan.transform.position = Vector3.SmoothDamp(capitan.transform.position, dest, ref velocity, smoothTime, speed, Time.deltaTime * DateManager.timeScale);
        DrawPath(dest);
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
            path.SetPosition(0, capitan.transform.position);
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
