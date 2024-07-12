using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvaliableBuildingButton : MonoBehaviour
{
    public Image icon;
    public Image background;
    public TMP_Text buildingName;

    public Building building;

    public void SetUp(Building building, ColonyStatus colonyStatus)
    {
        this.building = building;
        icon.sprite = building.icon;
        background.sprite = building.background;
        buildingName.text = building.buildingName;

        GetComponent<Button>().onClick.AddListener(() => colonyStatus.AddBuildingToQueue(building));
    }
}
