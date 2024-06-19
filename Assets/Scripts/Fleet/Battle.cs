using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Device;
using static Battle;
using static UnityEngine.GraphicsBuffer;

enum TargetSelection
{
    Random,
    Divide,
    Weakest,
    Strongest
}

public class Battle : MonoBehaviour
{
    private FriendlyFleet friendlyFleet;
    private EnemyFleet enemyFleet;

    public readonly float armorScalingFactor = .1f;

    public FleetStats friendlyFleetStats;
    public FleetStats enemyFleetStats;

    
    public void Initialize(FriendlyFleet ff, EnemyFleet ef)
    {
        // Separating enemy ships into screens and capitals

        friendlyFleetStats = GenerateFleetStats(ff);
        enemyFleetStats = GenerateFleetStats(ef);

        DateManager.instance.OnDateUpdate += BattleTick;
    }
    private FleetStats GenerateFleetStats(Fleet fleet)
    {
        FleetStats fleetStats = new FleetStats();
        fleetStats.myFleet = fleet;

        fleetStats.screens = fleet.composition.Where(s => FleetStats.CheckIfScreen(s)).ToList();
        fleetStats.capitals = fleet.composition.Where(s => !FleetStats.CheckIfScreen(s)).ToList();

        fleetStats.GetCapitalsChance();
        fleetStats.lightDamage = fleet.composition.Sum(s => s.stats.lightDamage);
        fleetStats.heavyDamage = fleet.composition.Sum(s => s.stats.heavyDamage);

        return fleetStats;
    }
    void BattleTick()
    {
        // Friendly fleet attacks enemy fleet
        if(friendlyFleetStats.screens.Count == 0 && friendlyFleetStats.capitals.Count == 0)
        {
            // Battle is over
            return;
        }
        
        if(enemyFleetStats.screens.Count == 0 && enemyFleetStats.capitals.Count == 0)
        {
            // Battle is over
            return;
        }

        Attack(ref friendlyFleetStats, ref enemyFleetStats);
        Attack(ref enemyFleetStats, ref friendlyFleetStats);
    }


    private void Attack(ref FleetStats source, ref FleetStats target)
    {
        Ship targetShip = GetTarget(ref source, ref target);
        float damage = CalculateDamage(targetShip, ref source);

        targetShip.stats.health -= damage;
        if (targetShip.stats.health <= 0)
        {
            target.ShipDestroyed(targetShip);
        }
    }
    private Ship GetTarget(ref FleetStats sourece, ref FleetStats target)
    {
        // TODO: Implement target selection, now it's random

        int screensCount = target.screens.Count;

        int seed = Random.Range(0, screensCount + target.chanceOfHittingCapitals);

        if (seed > screensCount)
        {
            // Hit random capital
            return target.capitals[Random.Range(0, target.capitals.Count)];
        }
        // Hit random screen
        return target.screens[Random.Range(0, screensCount)];
    }
    private float CalculateDamage(Ship target, ref FleetStats stats)
    {
        float dmg = 0;

        float lightDamageInfluence = 1 + target.stats.armor * armorScalingFactor;
        dmg += stats.lightDamage / lightDamageInfluence;

        dmg += stats.heavyDamage; // Heavy damage is always applied, there is no armor scaling

        return dmg;
    }
}