using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleBillboard : Billboard
{
    public Battle battle;

    // Derived text is used to display the friendly fleet
    public TMP_Text enemyFleetText;

    [SerializeField]
    private FleetIcons friendlyFleetIcons, enemyFleetIcons;

    protected override void Start()
    {
        return;
    }
    public void SetupBattle(Battle battle)
    {
        base.Start();

        target = new GameObject($"Battle billboard midpoint").transform;

        this.battle = battle;

        text.text = battle.friendlyFleet.objectName;
        enemyFleetText.text = battle.enemyFleet.objectName;

        UpdateBattle();
    }
    public void UpdateBattle()
    {
        friendlyFleetIcons.UpdateFleet(battle.friendlyFleet);
        enemyFleetIcons.UpdateFleet(battle.enemyFleet);
    }

    public void Click()
    {
        battle.CreateModal();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        target.position = (battle.friendlyFleet.capitan.transform.position + battle.enemyFleet.capitan.transform.position)/2;
    }
    void OnDestroy()
    {
        if(target != null) Destroy(target.gameObject);
    }
}