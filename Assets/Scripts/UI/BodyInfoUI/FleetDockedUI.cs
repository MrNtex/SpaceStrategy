using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FleetDockedUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text fleetName;
    public Fleet myFleet;

    public void SetFleet(Fleet fleet)
    {
        myFleet = fleet;
        fleetName.text = fleet.objectName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        myFleet.ButtonClicked();
    }
}
