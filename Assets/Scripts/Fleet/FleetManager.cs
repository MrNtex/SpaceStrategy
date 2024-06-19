using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public List<FriendlyFleet> fleets = new List<FriendlyFleet>();

    public static FleetManager instance;
    // Start is called before the first frame update

    public FriendlyFleet selectedFleet
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
    private FriendlyFleet _selectedFleet;


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
    
        Camera.main.GetComponent<CameraRightClick>().onRightClick += RightClick;
        bodyInfoUI = BodyInfoUI.instance;
    }
    void RightClick(GameObject target)
    {
        if (selectedFleet == null) return;


        if (target.CompareTag("CelestialBody") || target.CompareTag("Point"))
        {
            UpdateTarget(target);
            return;
        }

        if (target.CompareTag("Fleet"))
        {
            Fleet fleet = target.GetComponent<Fleet>();
            
            if (fleet is FriendlyFleet)
            {
                // TODO: Implement fleet merging
                return;
            }
            UpdateTarget(fleet.capitan);
        }
    }
    void UpdateTarget(GameObject dest)
    {
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
