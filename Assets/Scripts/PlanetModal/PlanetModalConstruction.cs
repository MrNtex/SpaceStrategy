using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetModalConstruction : MonoBehaviour
{
    [SerializeField]
    private Sprite emptySlot;

    [SerializeField]
    private Image[] slots;

    [SerializeField]
    private PlanetModal planetModal;

    [SerializeField]
    private Transform sideBar;

    [SerializeField]
    private GameObject avaliableBuildingInfo;
    private void OnEnable()
    {
        SetIcons();
        SetAvaliableBuildings();
    }
    public void SetIcons()
    {
        int i = 0;
        for(; i < planetModal.colonyStatus.buildings.Count; i++)
        {
            slots[i].sprite = planetModal.colonyStatus.buildings[i].icon;
        }
        for(; i < slots.Length; i++)
        {
            slots[i].sprite = emptySlot;
        }
    }
    public void SetAvaliableBuildings()
    {
        foreach(Building building in BuildingsManager.instance.avaliableBuildings)
        {
            AvaliableBuildingButton buildingButton = Instantiate(avaliableBuildingInfo, sideBar).GetComponent<AvaliableBuildingButton>();
            buildingButton.icon.sprite = building.icon;
            buildingButton.background.sprite = building.background;
            buildingButton.buildingName.text = building.buildingName;
            buildingButton.building = building;
        }
    }
}
