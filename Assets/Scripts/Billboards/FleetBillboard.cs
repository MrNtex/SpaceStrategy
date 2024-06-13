using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class FleetBillboard : Billboard
{
    [SerializeField]
    private List<Color> textColors;

    private FriendlyFleet fleet;

    private ObjectFocusHelper objectFocusHelper;


    [SerializeField]
    private FleetIcon[] fleetIcons;
    protected override void Start()
    {
        return;
    }
    public void SetupFleet()
    {
        minDistance = -1;

        base.Start();

        fleet = transform.parent.GetComponent<FriendlyFleet>(); // UI is a child of the fleet capitan, while the fleet info is the parent

        text.text = fleet.objectName;
        button.GetComponent<Button>().onClick.AddListener(() => fleet.ButtonClicked());
        UpdateFleet();

        // KTO TAK SPIERDOLIL TE SOURCES !!!!

        target = fleet.capitan.transform;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = target;
        source.weight = 1;

        if(positionConstraint.sourceCount == 0)
        {
            positionConstraint.AddSource(source);
            return;
        }
        positionConstraint.SetSource(0, source);
    }
    public void UpdateFleet()
    {
        Dictionary<ShipType, int> shipCount = new Dictionary<ShipType, int>();
        foreach (Ship ship in fleet.composition)
        {
            if (shipCount.ContainsKey(ship.type))
                shipCount[ship.type] += 1;
            else
            {
                shipCount.Add(ship.type, 1);
            }
            
        }
        foreach(FleetIcon icon in fleetIcons)
        {
            int count = 0;
            if (shipCount.TryGetValue(icon.type, out count) && count > 0)
            {
                icon.icon.SetActive(true);
                icon.text.text = shipCount[icon.type].ToString();
            }
            else
            {
                icon.icon.SetActive(false);
            }
        }
    }
    [Serializable]
    public struct FleetIcon
    {
        public ShipType type;
        public TMP_Text text;
        public GameObject icon;
    }
}
