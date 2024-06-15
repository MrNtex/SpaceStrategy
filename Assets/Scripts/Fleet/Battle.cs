using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    List<ShipStats> friendlyScreens = new List<ShipStats>();
    List<ShipStats> friendlyCapitals = new List<ShipStats>();

    List<ShipStats> enemyScreens = new List<ShipStats>();
    List<ShipStats> enemyCapitals = new List<ShipStats>();
    public void SetUp(FriendlyFleet ff, EnemyFleet ef)
    {
        foreach (Ship s in ff.composition)
        {
            if (CheckIfScreen(s))
            {
                friendlyScreens.Add(s.stats);
            }
            else
            {
                friendlyCapitals.Add(s.stats);
            }
        }

        foreach (Ship s in ef.composition)
        {
            if (CheckIfScreen(s))
            {
                enemyScreens.Add(s.stats);
            }
            else
            {
                enemyCapitals.Add(s.stats);
            }
        }

        DateManager.instance.OnDateUpdate += BattleTick;
    }
    private void Start()
    {
        DateManager.instance.OnDateUpdate += BattleTick;
    }

    private void BattleTick()
    {
        Debug.Log("Tick");
    }

    private bool CheckIfScreen(Ship s)
    {
        return s.type == ShipType.Fighter || s.type == ShipType.Destroyer || s.type == ShipType.LightCruiser;
    }
}
