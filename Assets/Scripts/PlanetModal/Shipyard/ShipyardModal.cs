using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardModal : PlanetModalPage
{
    public ColonyStatus colonyStatus;
    public BodyInfo bodyInfo;

    [Header("Docked Ships")]
    [SerializeField]
    private GameObject fleetsContainer;

    [SerializeField]
    private GameObject fleetContainerPrefab;

    [SerializeField]
    private RectTransform dockedFleetsRect;
    [SerializeField]
    private VerticalLayoutGroup layoutGroup;
    public override void Create(PlanetModal planetModal)
    {
        colonyStatus = planetModal.colonyStatus;  
        bodyInfo = planetModal.bodyInfo;

        UpdateFleets();
    }

    public override void OnColonyUpdate()
    {
        throw new System.NotImplementedException();
    }

    void UpdateFleets()
    {
        layoutGroup.enabled = false;

        DestroyAllChildren.DestroyAllChildrenOf(fleetsContainer.transform);


        foreach (var fleet in bodyInfo.fleetsOnOrbit)
        {
            GameObject fleetContainer = Instantiate(fleetContainerPrefab, fleetsContainer.transform);
            fleetContainer.GetComponent<ShipyardModalFleetContainer>().SetFleet(fleet);
        }

        StartCoroutine(UpdateLayout());
    }

    IEnumerator UpdateLayout()
    {
        yield return new WaitForFixedUpdate();
        LayoutRebuilder.MarkLayoutForRebuild(dockedFleetsRect);

        Debug.Log("Layout rebuild");

        layoutGroup.enabled = true;
    }
}
