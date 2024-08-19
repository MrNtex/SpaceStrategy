using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardModalFleetContainer : MonoBehaviour
{
    public FriendlyFleet fleet;
    public TMP_Text header;

    [SerializeField]
    private GameObject shipContainerPrefab;

    [SerializeField]
    private Transform shipContainerScreens, shipContainerCapitals;

    [SerializeField]
    private VerticalLayoutGroup layoutGroup;
    public void SetFleet(FriendlyFleet fleet)
    {
        this.fleet = fleet;
        header.text = fleet.name;

        layoutGroup.enabled = false;

        foreach (var ship in fleet.composition)
        {
            bool isScreen = FleetStats.CheckIfScreen(ship);
            Transform container = isScreen ? shipContainerScreens.transform : shipContainerCapitals.transform;
            
            GameObject shipObj = Instantiate(shipContainerPrefab, container);

            bool isCapitan = ship.prefab == fleet.capitan;
            
            if(isCapitan) shipObj.transform.SetAsFirstSibling();

            shipObj.GetComponent<ShipyardModalShipContainer>().SetShip(ship, isCapitan);
        }

        layoutGroup.enabled = true;
    }
}
