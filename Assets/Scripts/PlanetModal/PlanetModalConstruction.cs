using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetModalConstruction : PlanetModalPage
{
    [SerializeField]
    private Sprite emptySlot;

    [SerializeField]
    private BuildingButton[] slots;

    [SerializeField]
    private PlanetModal planetModal;

    [SerializeField]
    private Transform sideBar;

    [SerializeField]
    private GameObject avaliableBuildingInfo;
    public override void Create(PlanetModal planetModal)
    {
        this.planetModal = planetModal;

        SetIcons();
        SetAvaliableBuildings();
    }

    public override void OnColonyUpdate()
    {
        SetIcons();
    }
    public void SetIcons()
    {
        Debug.Log("Setting icons");
        int i = 0;
        for(; i < planetModal.colonyStatus.buildings.Count; i++)
        {
            slots[i].SetUp(planetModal.colonyStatus.buildings[i]);
        }
        foreach(ColonyStatus.Construction construction in planetModal.colonyStatus.constructionQueue)
        {
            slots[i].SetUp(construction);
            i++;
        }
        for(; i < slots.Length; i++)
        {
            slots[i].img.sprite = emptySlot;
        }
    }
    public void SetAvaliableBuildings()
    {
        foreach (Transform child in sideBar)
        {
            Destroy(child.gameObject);
        }


        foreach (Building building in BuildingsManager.instance.avaliableBuildings)
        {
            AvaliableBuildingButton buildingButton = Instantiate(avaliableBuildingInfo, sideBar).GetComponent<AvaliableBuildingButton>();
            buildingButton.SetUp(building, planetModal.colonyStatus);
        }
    }
}
