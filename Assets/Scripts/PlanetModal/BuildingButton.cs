using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Image img;
    public Image secondLayer;

    public Building building;
    public ColonyStatus.Construction construction;

    [SerializeField]
    private Material blackAndWhite;

    private void Awake()
    {
        img = GetComponent<Image>();
        secondLayer = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetUp(Building building)
    {
        img.material = null;
        img.sprite = building.icon;
        this.building = building;
        secondLayer.enabled = false;
    }
    public void SetUp(ColonyStatus.Construction construction)
    {
        img.sprite = construction.building.icon;
        img.material = blackAndWhite;

        secondLayer.sprite = construction.building.icon;
        secondLayer.enabled = true;
        this.construction = construction;
        construction.button = this;
    }
}
