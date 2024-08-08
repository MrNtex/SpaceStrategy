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
    [SerializeField]
    private BattleBillboard billboard;

    public List<Fleet> friendlyFleets = new List<Fleet>();
    public List<Fleet> enemyFleets = new List<Fleet>();

    public readonly float armorScalingFactor = .1f;

    public FleetStats friendlyFleetStats;
    public FleetStats enemyFleetStats;

    public delegate void ShipUpdate(Ship ship);
    public event ShipUpdate OnShipUpdate;

    public delegate void BattleEnd(Fleet winner);
    public event BattleEnd OnBattleEnd;

    private BattleModal battleModal;
    
    public void Initialize(List<FriendlyFleet> ff, List<EnemyFleet> ef)
    {
        // Separating enemy ships into screens and capitals
        friendlyFleets = ff.Cast<Fleet>().ToList();
        enemyFleets = ef.Cast<Fleet>().ToList();

        Debug.Log("Battle started");
        Debug.Log($"Enemy fleets: {enemyFleets.Count}");

        friendlyFleetStats = GenerateFleetStats(friendlyFleets);
        enemyFleetStats = GenerateFleetStats(enemyFleets);

        foreach(Fleet f in friendlyFleets)
        {
            f.fleetBillboard.gameObject.SetActive(false);
        }
        foreach (Fleet f in enemyFleets)
        {
            f.fleetBillboard.gameObject.SetActive(false);
        }

        billboard.SetupBattle(this);

        DateManager.instance.OnDateUpdate += BattleTick;
    }
    private FleetStats GenerateFleetStats(List<Fleet> fleets)
    {
        FleetStats fleetStats = new FleetStats();
        fleetStats.battle = this;
        fleetStats.myFleets = fleets;


        foreach(Fleet fleet in fleets)
        {
            foreach(Ship ship in fleet.composition)
            {
                fleetStats.lightDamage += ship.stats.lightDamage;
                fleetStats.heavyDamage += ship.stats.heavyDamage;

                if (FleetStats.CheckIfScreen(ship))
                {
                    fleetStats.screens.Add(ship);
                }
                else
                {
                    fleetStats.capitals.Add(ship);
                }
            }
        }

        return fleetStats;
    }
    void BattleTick()
    {
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
            target.ShipDestroyed(targetShip); // Get capitals chance is called in ShipDestroyed
            billboard.UpdateBattle();
        }
        OnShipUpdate?.Invoke(targetShip);
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

    public void FleetDestroyed(Fleet fleet)
    {

        fleet.UpdateFleet();

        if (fleet is EnemyFleet)
        {
            enemyFleets.Remove(fleet);
            if(enemyFleets.Count == 0)
            {
                EndBattle();
            }
        }
        else
        {
            friendlyFleets.Remove(fleet);
            if (friendlyFleets.Count == 0)
            {
                EndBattle();
            }
        }
    }

    public void EndBattle()
    {
        DateManager.instance.OnDateUpdate -= BattleTick;

        Debug.Log("Battle ended");

        foreach(Fleet friendlyFleet in friendlyFleets)
        {
            friendlyFleet.SetFleetStatus(FleetStatus.Idle);
            friendlyFleet.fleetBillboard.gameObject.SetActive(true);
        }

        foreach(Fleet enemyFleet in enemyFleets)
        {
            enemyFleet.SetFleetStatus(FleetStatus.Idle);
            enemyFleet.fleetBillboard.gameObject.SetActive(true);
        }

        //OnBattleEnd?.Invoke(loser == enemyFleet ? friendlyFleet : enemyFleet); // Had to invert the loser, because the loser is the one that is destroyed

        Destroy(gameObject);
    }

    public void CreateModal()
    {
        battleModal = Instantiate(BattlesManager.instance.battleModalPrefab, MenusManager.Instance.mainCanvas.transform).GetComponent<BattleModal>();

        battleModal.Create(this);
    }
}