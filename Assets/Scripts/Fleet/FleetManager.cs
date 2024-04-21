using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public Fleet[] fleets;

    public static FleetManager instance;
    // Start is called before the first frame update

    public Fleet selectedFleet;

    private void Awake()
    {
        instance = this;

        Camera.main.GetComponent<CameraRightClick>().onRightClick += UpdateTarget;
    }
    void UpdateTarget(Vector3 dest)
    {
        Debug.Log("Updating target to: " + dest);
        if (selectedFleet != null)
            selectedFleet.SetDestination(dest);
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
