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

    private BodyInfoUI bodyInfoUI;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one FleetManager in the scene");
        }
    
        Camera.main.GetComponent<CameraRightClick>().onRightClick += UpdateTarget;
        bodyInfoUI = BodyInfoUI.instance;
    }
    void UpdateTarget(GameObject dest)
    {
        if (selectedFleet == null) return;

        selectedFleet.SetDestination(dest);

        bodyInfoUI.SetBody(selectedFleet);
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetSelectedFleet(null);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            selectedFleet.SetStatus(FleetStatus.Idle);
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
    OnOrbit,
    Attacking,
    Fleeing
}
