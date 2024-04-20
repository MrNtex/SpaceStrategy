using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FleetBillboard : Billboard
{
    [SerializeField]
    private List<Color> textColors;

    private Fleet fleet;

    private ObjectFocusHelper objectFocusHelper;


    [SerializeField]
    private FleetIcon[] fleetIcons;
    protected override void Start()
    {
        minDistance = -1;
        base.Start();
        SetupFleet();
    }
    private void SetupFleet()
    {
        fleet = target.GetComponent<Fleet>();
        text.text = fleet.fleetName;
        button.GetComponent<Button>().onClick.AddListener(() => objectFocusHelper.Focus());
        UpdateFleet();
    }
    public void UpdateFleet()
    {
        Dictionary<ShipType, int> shipCount = new Dictionary<ShipType, int>();
        foreach (Ship ship in fleet.composition)
        {
            shipCount.Add(ship.type, ship.count);
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
