using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyFleetPriority
{
    Normal, // No priority, if fleet is in range, attack
    SearchAndDestroy, // Will prioritize attacking fleets
    DestroyColony, // Will prioritize attacking colonies
    Support, // Will prioritize supporting other waves
    Flee
}
public class EnemyFleetAI : MonoBehaviour
{
    public float range = 300;

    EnemyFleetPriority priority = EnemyFleetPriority.Normal;


    private void Start()
    {
        
    }
    void CheckForTargets(float fleetSearchRange)
    {
        Fleet targetFleet = TargetFleet(fleetSearchRange);

        if (targetFleet != null)
        {
            // Attack
            return;
        }


        GameObject targetColony = FindTheClosestColony();
        if (targetColony != null)
        {
            // Move to target
            return;
        }
        // Something went wrong
        Debug.LogError($"{this} - No target found");
    }

    private Fleet TargetFleet(float fleetSearchRange)
    {
        foreach (Fleet fleet in FleetManager.instance.fleets)
        {
            float dist = Vector3.Distance(transform.position, fleet.transform.position);
            if (dist < fleetSearchRange)
            {
                return fleet;
            }
        }
        return null;
    }

    GameObject FindTheClosestColony()
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("CelestialBody");
        GameObject closestFleet = null;
        foreach (GameObject planet in bodies)
        {
            BodyInfo bodyInfo = planet.GetComponent<BodyInfo>();
            if(bodyInfo == null)
            {
                Debug.LogWarning(planet + " is marked, but doesn't have the BodyInfo attached");
                continue;
            }
            if (CanBeAttacked(bodyInfo.status) && Vector3.Distance(transform.position, planet.transform.position) < Vector3.Distance(transform.position, closestFleet.transform.position))
            {
                closestFleet = planet;
            }
        }
        return closestFleet;
    }
    private bool CanBeAttacked(BodyStatusType bst)
    {
        switch (bst)
        {
            case BodyStatusType.Colonized:
                return true;
            case BodyStatusType.Specialized:
                return true;
            default:
                return false;
        }
    }
}
