using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public Fleet[] fleets;

    public static FleetManager instance;
    // Start is called before the first frame update

    public Fleet selectedFleet
    {
        get
        {
            return _selectedFleet;
        }
        set
        {
            if (_selectedFleet != null)
            {
                _selectedFleet.path.startColor = normal;
                _selectedFleet.path.endColor = normal;
            }
            _selectedFleet = value;
            if (_selectedFleet != null)
            {
                _selectedFleet.path.startColor = focused;
                _selectedFleet.path.endColor = focused;
            }
        }
    }
    [SerializeField]
    private Fleet _selectedFleet;


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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectedFleet = null;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            selectedFleet.SetFleetStatus(FleetStatus.Idle);
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
