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

    public GameObject target;

    private void Start()
    {
        target = CheckForTargets(range);
        Debug.Log(target);
    }
    GameObject CheckForTargets(float fleetSearchRange)
    {
        Fleet targetFleet = TargetFleet(fleetSearchRange);

        if (targetFleet != null)
        {
            // Attack
            return targetFleet.capitan;
        }


        GameObject targetColony = FindTheClosestColony();
        if (targetColony != null)
        {
            // Move to target
            return targetColony;
        }
        // Something went wrong
        Debug.LogError($"{this} - No target found");
        return null;
    }

    private Fleet TargetFleet(float fleetSearchRange)
    {
        Fleet closest = null;
        foreach (Fleet fleet in FleetManager.instance.fleets)
        {
            float dist = Vector3.Distance(transform.position, fleet.capitan.transform.position);
            Debug.Log(dist);
            if (dist < fleetSearchRange)
            {
                if (closest == null)
                {
                    closest = fleet;
                    continue;
                }
                if(Vector3.Distance(transform.position, fleet.capitan.transform.position) < Vector3.Distance(transform.position, closest.capitan.transform.position))
                {
                    closest = fleet;
                }
            }
        }
        return closest;
    }

    GameObject FindTheClosestColony()
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("CelestialBody");
        GameObject closestBody = null;
        foreach (GameObject planet in bodies)
        {
            BodyInfo bodyInfo = planet.GetComponent<BodyInfo>();
            if(bodyInfo == null)
            {
                Debug.LogWarning(planet + " is marked, but doesn't have the BodyInfo attached");
                continue;
            }
            if (closestBody == null)
            {
                closestBody = planet;
                continue;
            }
            if (CanBeAttacked(bodyInfo.status) && Vector3.Distance(transform.position, planet.transform.position) < Vector3.Distance(transform.position, closestBody.transform.position))
            {
                closestBody = planet;
            }
        }
        return closestBody;
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
