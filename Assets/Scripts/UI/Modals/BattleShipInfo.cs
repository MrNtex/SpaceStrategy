using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleShipInfo : MonoBehaviour
{
    private Ship myShip;

    [SerializeField]
    private Image health;
    [SerializeField]
    private Image shield;

    [SerializeField]
    private TMP_Text shipName;

    public void Create()
    {

    }

    public void UpdateStats()
    {
        health.fillAmount = myShip.stats.health / myShip.stats.maxHealth;
        shield.fillAmount = myShip.stats.shield / myShip.stats.maxShield;
    }
}
