using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public Fleet[] fleets;

    public static FleetManager instance;
    // Start is called before the first frame update

    public Fleet selectedFleet;

    public Color focused, normal;

    private void Awake()
    {
        instance = this;

        Camera.main.GetComponent<CameraRightClick>().onRightClick += UpdateTarget;
    }
    void UpdateTarget(GameObject dest)
    {
        if (selectedFleet != null)
            selectedFleet.SetDestination(dest);
    }
    public void SetSelectedFleet(Fleet fleet)
    {
        if(selectedFleet != null)
        {
            selectedFleet.path.startColor = normal;
            selectedFleet.path.endColor = normal;
        }
        selectedFleet = fleet;
        if(selectedFleet != null){
            selectedFleet.path.startColor = focused;
            selectedFleet.path.endColor = focused;
        }
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
