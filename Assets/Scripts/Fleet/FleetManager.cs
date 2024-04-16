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
