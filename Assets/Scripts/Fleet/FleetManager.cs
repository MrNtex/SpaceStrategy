using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    public List<FriendlyFleet> fleets = new List<FriendlyFleet>();

    public static FleetManager instance;
    // Start is called before the first frame update

    public Sprite haltFleet;

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

    [Header("Path Colors")]
    public Color normalPath; // This way, because the header would break
    public Color mergePath, attackPath;

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
    
        CameraControler.mainCamera.GetComponent<CameraRightClick>().onRightClick += RightClick;
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
                if(fleet == selectedFleet) return;
                FriendlyFleet friendlyFleet = fleet as FriendlyFleet;

                MenusManager.Instance.mainComboBox.Show(new ComboBoxItem[] { new ComboBoxItem("Merge", () => UpdateTarget(fleet.capitan, FleetStatus.Merging)), new ComboBoxItem("Follow", () => UpdateTarget(fleet.capitan, FleetStatus.Moving)) });


                return;
            }
            UpdateTarget(fleet.capitan);
        }
    }
    void UpdateTarget(GameObject dest, FleetStatus nextStatus = FleetStatus.Moving)
    {
        selectedFleet.SetDestination(dest, nextStatus);

        BodyInfoUI.instance.SetBody(selectedFleet);
    }

    public void MergeFleets(FriendlyFleet fleet, FriendlyFleet selectedFleet)
    {
        fleets.Remove(fleet);

        selectedFleet.Merge(fleet);

        Destroy(fleet.gameObject);
    }

    public Color GetPathColor(Fleet fleet)
    {
        switch (fleet.status)
        {
            case FleetStatus.Moving:
            {
                if (fleet.destination.CompareTag("Ship"))
                {
                    Fleet dest = fleet.destination.GetComponentInParent<Fleet>();
                    //ENEMY FLEET
                    if (dest is EnemyFleet)
                    {
                        return attackPath;
                    }
                }
                return normalPath;
            }
            case FleetStatus.Merging:
                return mergePath;
            default:
                return normalPath;
        }
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
