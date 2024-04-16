using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public Fleet[] fleets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateTarget(Fleet fleet)
    {

    }

    private void Update()
    {
        
    }
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
    Attacking,
    Fleeing
}
[System.Serializable]
public struct Fleet
{
    public string name;

    public GameObject capitan;
    public GameObject[] ships;

    public Vector3 target;

    public FleetStatus status;

    Fleet(string name, GameObject capitan, GameObject[] ships, Vector3 target, FleetStatus status = FleetStatus.Idle)
    {
        this.name = name;
        this.capitan = capitan;
        this.ships = ships;
        this.target = target;
        this.status = status;
    }
}
