using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlesManager : MonoBehaviour
{
    public static BattlesManager instance;

    public Dictionary<(FriendlyFleet, EnemyFleet), Battle> activeBattles = new Dictionary<(FriendlyFleet, EnemyFleet), Battle>();

    [SerializeField]
    private GameObject battlePrefab;
    private void Awake()
    {
        instance = this;
    }

    public void AddBattle(FriendlyFleet ff, EnemyFleet ef)
    {
        if (activeBattles.ContainsKey((ff, ef)))
        {
            Debug.LogWarning($"Battle between {ff} and {ef} already exists");
            return;
        }

        ff.SetFleetStatus(FleetStatus.Fighting);
        ef.SetFleetStatus(FleetStatus.Fighting);

        GameObject battleGO = Instantiate(battlePrefab, ff.transform);
        Battle battle = battleGO.GetComponent<Battle>();
        battle.Initialize(ff, ef);

        activeBattles.Add((ff, ef), battle);
    }
}
