using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FleetIcons : MonoBehaviour
{
    [SerializeField]
    private FleetIcon[] fleetIcons;

    public void UpdateFleet(Fleet fleet)
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
        foreach (FleetIcon icon in fleetIcons)
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
    public void UpdateFleet(List<Fleet> fleets)
    {
        Dictionary<ShipType, int> shipCount = new Dictionary<ShipType, int>();
        foreach (Fleet fleet in fleets)
        {
            // Iterate over each ship in the fleet's composition
            foreach (Ship ship in fleet.composition)
            {
                if (shipCount.ContainsKey(ship.type))
                    shipCount[ship.type] += 1;
                else
                    shipCount.Add(ship.type, 1);
            }
        }
        foreach (FleetIcon icon in fleetIcons)
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
