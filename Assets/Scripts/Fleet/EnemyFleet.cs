using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleet : Fleet
{
    public EnemyFleetAI enemyFleetAI;
    public float fightingRange = 50f;

    protected override void Start()
    {
        base.Start();
        enemyFleetAI = GetComponent<EnemyFleetAI>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(status == FleetStatus.Moving)
        {
            float distance = Vector3.Distance(capitan.transform.position, destination.transform.position);
            if(distance < fightingRange && enemyFleetAI.targetInfo is FriendlyFleet)
            {
                status = FleetStatus.Fighting;
                FriendlyFleet targetFleet = enemyFleetAI.targetInfo as FriendlyFleet;
                targetFleet.SetFleetStatus(FleetStatus.Fighting);
            }
        }
    }
}
