using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ColonyStatus;

public class BuildingButton : MonoBehaviour
{
    public Image img;
    public Image secondLayer;

    public Building building;
    public ColonyStatus.Construction construction;

    [SerializeField]
    private Material blackAndWhite;

    [SerializeField]
    private Color constructionGray;

    private void Awake()
    {
        img = GetComponent<Image>();
        secondLayer = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetUp(PlacedBuilding building)
    {
        if(!building.active || building.notEnoughEnergy) img.material = blackAndWhite;
        else img.material = null;
        img.sprite = building.building.icon;
        img.color = Color.white;

        this.building = building.building;
        secondLayer.enabled = false;
    }

    public void SetUp(ColonyStatus.Construction construction)
    {
        img.sprite = construction.building.icon;
        img.material = blackAndWhite;

        if(construction.notEnoughEnergy) img.color = Color.red;
        else img.color = constructionGray;


        secondLayer.sprite = construction.building.icon;
        secondLayer.enabled = true;
        secondLayer.fillAmount = 0;
        this.construction = construction;
        construction.button = this;
    }
    public void UpdateFill(float fill)
    {
        secondLayer.fillAmount = fill;
    }
}
