using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField]
    List<Ship> friendlyScreens = new List<Ship>();
    [SerializeField]
    List<Ship> friendlyCapitals = new List<Ship>();

    [SerializeField]
    List<Ship> enemyScreens = new List<Ship>();
    [SerializeField]
    List<Ship> enemyCapitals = new List<Ship>();
    public void Initialize(FriendlyFleet ff, EnemyFleet ef)
    {
        // Separating enemy ships into screens and capitals

        friendlyScreens = ff.composition.Where(s => CheckIfScreen(s)).ToList();
        friendlyCapitals = ff.composition.Where(s => !CheckIfScreen(s)).ToList();

        
        enemyScreens = ef.composition.Where(s => CheckIfScreen(s)).ToList();
        enemyCapitals = ef.composition.Where(s => !CheckIfScreen(s)).ToList();

        
        DateManager.instance.OnDateUpdate += BattleTick;
    }
    void BattleTick()
    {

    }
    private bool CheckIfScreen(Ship s)
    {
        return s.type == ShipType.Fighter || s.type == ShipType.Destroyer || s.type == ShipType.LightCruiser;
    }
}
