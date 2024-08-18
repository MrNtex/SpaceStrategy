using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardModalShipContainer : MonoBehaviour
{
    public Ship ship;

    [SerializeField]
    private Image icon, crown;

    [SerializeField]
    private TMPro.TMP_Text nameText;

    public void SetShip(Ship ship, bool isCapitan)
    {
        this.ship = ship;

        icon.sprite = ShipIconUtil.shipIcons[ship.type];
        nameText.text = ship.shipName;

        crown.gameObject.SetActive(isCapitan);
    }
}
