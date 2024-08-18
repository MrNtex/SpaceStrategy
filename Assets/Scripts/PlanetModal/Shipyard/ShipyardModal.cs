using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipyardModal : PlanetModalPage
{
    public ColonyStatus colonyStatus;
    public BodyInfo bodyInfo;

    [Header("Docked Ships")]
    [SerializeField]
    private GameObject fleetsContainer;

    [SerializeField]
    private GameObject fleetContainerPrefab;
    public override void Create(PlanetModal planetModal)
    {
        colonyStatus = planetModal.colonyStatus;  
        bodyInfo = planetModal.bodyInfo;

        UpdateFleets();
        foreach (var ship in bodyInfo.fleetsOnOrbit)
        {
            Debug.Log(ship.name);
        }
    }

    public override void OnColonyUpdate()
    {
        throw new System.NotImplementedException();
    }

    void UpdateFleets()
    {
        DestroyAllChildren.DestroyAllChildrenOf(fleetsContainer.transform);

        foreach (var fleet in bodyInfo.fleetsOnOrbit)
        {
            GameObject fleetContainer = Instantiate(fleetContainerPrefab, fleetsContainer.transform);
            fleetContainer.GetComponent<ShipyardModalFleetContainer>().SetFleet(fleet);
        }
    }
}
