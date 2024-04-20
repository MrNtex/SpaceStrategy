using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FleetBillboard : Billboard
{
    [SerializeField]
    private List<Color> textColors;

    private Fleet fleet;

    private ObjectFocusHelper objectFocusHelper;
    protected override void Start()
    {
        minDistance = -1;
        base.Start();
        SetupFleet();
    }
    private void SetupFleet()
    {
        fleet = target.GetComponent<Fleet>();
        text.text = fleet.fleetName;
        button.GetComponent<Button>().onClick.AddListener(() => objectFocusHelper.Focus());
    }
}
