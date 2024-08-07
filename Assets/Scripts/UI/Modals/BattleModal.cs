using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleModal : Modal
{
    private Battle battle;

    public Dictionary<Ship, BattleShipInfo> shipInfos = new Dictionary<Ship, BattleShipInfo>();

    [SerializeField]
    private Transform friendlyShipsScreens, friendlyShipsCapitals, enemyShipsCapitals, enemyShipsScreens;

    [SerializeField]
    private GameObject battleShipInfoPrefab;

    [SerializeField]
    private TMP_Text screeningFriendly, screeningEnemy;
    public void Create(Battle b)
    {
        MenusManager.activeModals.Add(gameObject);

        battle = b;

        CreateBattleShipInfos(b.friendlyFleetStats.screens, friendlyShipsScreens);
        CreateBattleShipInfos(b.friendlyFleetStats.capitals, friendlyShipsCapitals);
        CreateBattleShipInfos(b.enemyFleetStats.screens, enemyShipsScreens);
        CreateBattleShipInfos(b.enemyFleetStats.capitals, enemyShipsCapitals);

        b.OnShipUpdate += UpdateShip;
        b.OnBattleEnd += Close;

        screeningFriendly.text = battle.friendlyFleetStats.chanceOfHittingCapitals.ToString();
        screeningEnemy.text = battle.enemyFleetStats.chanceOfHittingCapitals.ToString();
    }

    public void UpdateShip(Ship ship)
    {
        if (ship.stats.health <= 0)
        {
            shipInfos[ship].Destroyed(); // Can be reduced to Destroy(shipInfos[ship].gameObject), but might be useful to have a Destroyed method in the future
            shipInfos.Remove(ship);

            return;
        }
        shipInfos[ship].UpdateStats();

        screeningFriendly.text = battle.friendlyFleetStats.chanceOfHittingCapitals.ToString();
        screeningEnemy.text = battle.enemyFleetStats.chanceOfHittingCapitals.ToString();
    }

    void CreateBattleShipInfos(List<Ship> ships, Transform parent)
    {
        foreach (Ship ship in ships)
        {
            BattleShipInfo bsi = Instantiate(battleShipInfoPrefab, parent).GetComponent<BattleShipInfo>();
            bsi.Create(ship);
            shipInfos.Add(ship, bsi);
        }
    }

    public void Close(Fleet fleet)
    {
        Close();
    }
    public override void OnDestroy()
    {
        Close();

        battle.OnShipUpdate -= UpdateShip;
        battle.OnBattleEnd -= Close;
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
