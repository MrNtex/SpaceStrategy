using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlesManager : MonoBehaviour
{
    public static BattlesManager instance;

    public Dictionary<(FriendlyFleet, EnemyFleet), Battle> activeBattles = new Dictionary<(FriendlyFleet, EnemyFleet), Battle>();

    [SerializeField]
    private GameObject battlePrefab;

    public GameObject battleModalPrefab;
    private void Awake()
    {
        instance = this;
    }
    public void AddBattle(FriendlyFleet ff, EnemyFleet ef)
    {
        AddBattle(new List<FriendlyFleet> { ff }, new List<EnemyFleet> { ef });
    }
    public void AddBattle(List<FriendlyFleet> ff, List<EnemyFleet> ef)
    {


        foreach (FriendlyFleet f in ff)
        {
            f.SetFleetStatus(FleetStatus.Fighting);
            f.battleTarget = ef[0];
            f.FindFightDestination();
        }
        foreach(EnemyFleet e in ef)
        {
            e.SetFleetStatus(FleetStatus.Fighting);
            e.battleTarget = ff[0];
            e.FindFightDestination();
        }

        GameObject battleGO = Instantiate(battlePrefab, ff[0].transform);
        Battle battle = battleGO.GetComponent<Battle>();
        battle.Initialize(ff, ef);

        activeBattles.Add((ff[0], ef[0]), battle); // To do fix it
        Debug.LogWarning("Remember to fix the key in AddBattle method in BattlesManager.cs");
    }

    public void RemoveBattle(FriendlyFleet ff, EnemyFleet ef)
    {
        if (!activeBattles.ContainsKey((ff, ef)))
        {
            Debug.LogWarning($"Battle between {ff} and {ef} does not exist");
            return;
        }

        activeBattles.Remove((ff, ef));
    }
}
