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

    public FriendlyFleet friendlyFleet;
    public EnemyFleet enemyFleet;

    public readonly float armorScalingFactor = .1f;

    public FleetStats friendlyFleetStats;
    public FleetStats enemyFleetStats;

    public delegate void ShipUpdate(Ship ship);
    public event ShipUpdate OnShipUpdate;
    
    public void Initialize(FriendlyFleet ff, EnemyFleet ef)
    {
        // Separating enemy ships into screens and capitals
        friendlyFleet = ff;
        enemyFleet = ef;


        friendlyFleetStats = GenerateFleetStats(ff);
        enemyFleetStats = GenerateFleetStats(ef);

        friendlyFleet.fleetBillboard.gameObject.SetActive(false);
        enemyFleet.fleetBillboard.gameObject.SetActive(false);

        billboard.SetupBattle(this);

        DateManager.instance.OnDateUpdate += BattleTick;
    }
    private FleetStats GenerateFleetStats(Fleet fleet)
    {
        FleetStats fleetStats = new FleetStats();
        fleetStats.battle = this;
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

    public void EndBattle(Fleet loser)
    {
        DateManager.instance.OnDateUpdate -= BattleTick;

        Debug.Log("Battle ended");

        if(friendlyFleet != null)
        {
            friendlyFleet.SetFleetStatus(FleetStatus.Idle);
            friendlyFleet.fleetBillboard.gameObject.SetActive(true);
        }

        if(enemyFleet != null)
        {
            enemyFleet.SetFleetStatus(FleetStatus.Idle);
            enemyFleet.fleetBillboard.gameObject.SetActive(true);
        }

        Destroy(gameObject);

    }

    public void CreateModal()
    {
        BattleModal modal = Instantiate(BattlesManager.instance.battleModalPrefab, MenusManager.Instance.mainCanvas.transform).GetComponent<BattleModal>();

        modal.Create(this);
    }
}