using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class FleetBillboard : Billboard
{
    [SerializeField]
    private List<Color> textColors;

    public Fleet fleet;

    private ObjectFocusHelper objectFocusHelper;


    [SerializeField]
    private FleetIcon[] fleetIcons;

    [SerializeField]
    private Transform iconsParent;
    [SerializeField]
    private GameObject iconPrefab;
    public void SetUpFleet()
    {

        minDistance = -1;

        base.Start();

        fleet = transform.parent.GetComponent<Fleet>(); // UI is a child of the fleet capitan, while the fleet info is the parent

        text.text = fleet.objectName;
        button.GetComponent<Button>().onClick.AddListener(() => fleet.ButtonClicked());

        CreateIcons();

        UpdateFleet();

        // KTO TAK SPIERDOLIL TE SOURCES !!!!


        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = target;
        source.weight = 1;

        if (positionConstraint.sourceCount == 0)
        {
            positionConstraint.AddSource(source);
            return;
        }
        positionConstraint.SetSource(0, source);
    }
    void CreateIcons()
    {
        List<FleetIcon> icons = new List<FleetIcon>();
        foreach(KeyValuePair<ShipType, Sprite> kvp in ShipIconUtil.shipIcons)
        {
            GameObject go = Instantiate(iconPrefab, iconsParent);
            go.GetComponent<Image>().sprite = kvp.Value;

            go.SetActive(false);

            icons.Add(new FleetIcon(kvp.Key, go.GetComponentInChildren<TMP_Text>(), go));
        }
        fleetIcons = icons.ToArray();
    }
    public void UpdateFleet()
    {
        target = fleet.capitan.transform;

        Dictionary<ShipType, int> shipCount = new Dictionary<ShipType, int>();
        foreach (Ship ship in fleet.composition)
        {
            if (shipCount.ContainsKey(ship.type))
                shipCount[ship.type] += 1;
            else
            {
                shipCount.Add(ship.type, 1);
            }
            
        }
        foreach(FleetIcon icon in fleetIcons)
        {
            int count = 0;
            if (shipCount.TryGetValue(icon.type, out count) && count > 0)
            {
                icon.icon.SetActive(true);
                icon.text.text = shipCount[icon.type].ToString();
            }
            else
            {
                icon.icon.SetActive(false);
            }
        }
    }
    [Serializable]
    public struct FleetIcon
    {
        public ShipType type;
        public TMP_Text text;
        public GameObject icon;

        public FleetIcon(ShipType type, TMP_Text text, GameObject icon)
        {
            this.type = type;
            this.text = text;
            this.icon = icon;
        }
    }
}
