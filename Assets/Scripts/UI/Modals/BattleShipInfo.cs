using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleShipInfo : MonoBehaviour
{
    private Ship myShip;

    [SerializeField]
    private Image shipIcon;

    [SerializeField]
    private Image health;
    [SerializeField]
    private Image shield;

    [SerializeField]
    private TMP_Text shipName;

    public void Create(Ship ship)
    {
        myShip = ship;
        shipIcon.sprite = ShipIconUtil.shipIcons[myShip.type];
        shipName.text = myShip.shipName;
        UpdateStats();
    }

    public void UpdateStats()
    {
        health.fillAmount = myShip.stats.health / myShip.stats.maxHealth;
        if(myShip.stats.maxShield > 0) shield.fillAmount = myShip.stats.shield / myShip.stats.maxShield;
        else shield.fillAmount = 0;
    }

    public void Destroyed()
    {
        Destroy(gameObject);
    }
}
