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
            if(composition.Count == 0) return null;
            if(capitanIndex < 0 || capitanIndex >= composition.Count) return composition[0].prefab;
            return composition[capitanIndex].prefab;
        }
    }
    [SerializeField]
    private int capitanIndex = 0;
    [SerializeField]
    public GameObject destination;
    protected ObjectInfo destinationInfo;

    [HideInInspector]
    public float destinationOffset = 1;

    public FleetStatus status;

    [Header("Fleet stats")]

    public float speed = 15.0f;
    public float smoothTime = 0.3F; // Acceleration time

    public float maneuverabilityPenalty = 0.15f; // The multiplier of the angle modifier when ship has to rotate quickly, should be greater than 0
    public float minSpeed = 5.0f;

    public FleetBillboard fleetBillboard;

    public GameObject onOrbit;

    protected GameObject point;


    public LineRenderer path;
    protected const float lrOffset = 2.5f;

    private Transform mainCamera;

    public Battle battle;
    public float fightingRange = 50f;

    [SerializeField]
    private GameObject expolsionPartilcles;

    protected virtual void Start()
    {
        path = GetComponent<LineRenderer>();

        fleetBillboard.SetUpFleet();
        UpdateFleet(false);

        point = new GameObject($"{objectName}'s Point");

        mainCamera = CameraControler.mainCamera.transform;
    }

    public override void ButtonClicked()
    {
        if (destination == null)
        {
            objectFocusHelper.SetParent(capitan.transform, capitan.transform.position);
        }
        else
        {
            objectFocusHelper.SetParent(capitan.transform, destination.transform.position);
        }
        
        BodyInfoUI.instance.SetBody(this);

        cameraFocus.FocusOn(objectFocusHelper, FleetManager.instance.selectedFleet == this);
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
            if(dest.CompareTag("Ship")) destinationInfo = dest.transform.parent.GetComponent<ObjectInfo>();
            else destinationInfo = dest.GetComponent<ObjectInfo>();
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
    public virtual void AddToFleet(Ship ship, bool update = true)
    {
        ship.prefab = Instantiate(ship.prefab, transform);

        composition.Add(ship);
        if (update) UpdateFleet();
    }
    public virtual void UpdateFleet(bool billboard = true)
    {
        if(composition.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        if(capitan == null)
        {
            capitan = composition[0].prefab;
        }

        FleetFormationHelper.instance.SetFormation(FleetFormation.Triangle, composition.ToArray(), capitan);
        if(billboard) fleetBillboard.UpdateFleet();
    }

    public void RemoveFromFleet(Ship ship, bool inBattle)
    {
        bool isCapitan = ship.prefab == capitan;
        if (inBattle)
        {
            Destroy(Instantiate(expolsionPartilcles, ship.prefab.transform.position, Quaternion.identity), 2.0f);
        }
        Destroy(ship.prefab);
        composition.Remove(ship);
        if (isCapitan && composition.Count > 0)
        {
            capitan = composition[0].prefab;
        }
        UpdateFleet();
    }

    public virtual void DrawPath(Vector3 dest) { }
    public virtual void SetFleetStatus(FleetStatus status)
    {
        this.status = status;
    }

    private void OnDestroy()
    {
        Destroy(point);
    }
}
[System.Serializable]
public class Ship
{
    public ShipType type;

    public string shipName;

    //public int count;
    public Vector3 velocity;
    public GameObject prefab;
    public Vector3 myOffset;

    public ShipStats stats;
}
[System.Serializable]
public class ShipStats
{
    public float speed;
    public float maneuverabilityPenalty;

    public float lightDamage; // Damage dealt to light ships
    public float heavyDamage; // Damage dealt to capital ships

    public float maxHealth;
    public float maxShield;

    public float health;
    public float armor;
    public float shield;

    [Range(0, 1)]
    public float reliability; // The chance of the ship to take critical damage
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
    Fighting,
    Fleeing
}
public enum ShipType
{
    Fighter,
    Destroyer,
    LightCruiser,
    HeavyCruiser,
    Battleship,
    Dreadnought
}
