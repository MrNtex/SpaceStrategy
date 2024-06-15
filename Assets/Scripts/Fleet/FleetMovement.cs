using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fleet : ObjectInfo
{

    private Vector3 capitanVelocity = Vector3.zero;

    private const float forceManueverDistance = 50f;

    [Header("Formation")]
    public bool forceManeuver = false;

    protected virtual void FixedUpdate()
    {
        if(status == FleetStatus.Fighting && battleTarget != null)
        {
            FightMovement();
            return;
        }
        if (status == FleetStatus.Moving && destination != null)
        {
            if (destination.CompareTag("Point"))
            {
                FlyCapitan(destination.transform.position);
            }
            else
            {
                Vector3 near = destination.transform.position - capitan.transform.position;
                near -= near.normalized;
                near += capitan.transform.position;
                FlyCapitan(near);
            }
            
            
        }
        for (int i = 0; i < composition.Count; i++)
        {
            FlyTowards(i);
        }
    }
    void FlyTowards(int idx)
    {
        Transform t = composition[idx].prefab.transform;

        if (t == capitan)
        {
            return;
        }

        float angle = capitan.transform.rotation.eulerAngles.y;

        Vector3 offset = Quaternion.Euler(0, angle, 0) * composition[idx].myOffset;
        Vector3 dest = capitan.transform.position + offset;

        if (Vector3.Distance(mainCamera.position, t.position) > 1000f)
        {
            // Perform simplified calculations
            t.position = dest;
            t.rotation = capitan.transform.rotation;

            return;
        }

        if (Vector3.Distance(t.position, dest) < .1f)
        {
            return;
        }
        CalculateMovment(dest, composition[idx]);

        return;
    }
    void FlyCapitan(Vector3 dest)
    {
        // Capitan version

        if (Vector3.Distance(capitan.transform.position, dest) < destinationOffset)
        {
            if (destination.CompareTag("CelestialBody"))
            {
                gameObject.transform.SetParent(destination.transform);
                onOrbit = destination;
            }
            if(status == FleetStatus.Moving) // Do not change status while fighing
                SetFleetStatus(FleetStatus.Idle);
            return;
        }

        DrawPath(dest);
        CalculateMovment(dest, composition[capitanIndex]);
    }

    public void CalculateMovment(Vector3 dest, Ship ship)
    {
        if (DateManager.timeScale == 0) return;

        Transform t = ship.prefab.transform;

        if (DateManager.timeScale > 50)
        {
            t.position = Vector3.MoveTowards(t.position, dest, speed * DateManager.timeScale * Time.deltaTime);
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(dest - t.position);

        float distance = Vector3.Distance(t.position, dest);

        Vector3 dir = dest - t.position;
        float angle = Vector3.Angle(t.forward, dir); // The resulting angle ranges from 0 to 180.

        float maneuverSpeed = speed * DateManager.timeScale;

        if (status == FleetStatus.Fighting || (distance > forceManueverDistance && !forceManeuver))
        {
            //Speed up rotation when getting closer
            float angularMultiplier = .55f;
            if (distance < 400)
            {
                float ratio = 1 - (distance - forceManueverDistance) / (400 - forceManueverDistance);
                angularMultiplier = Mathf.Lerp(.55f, 2, ratio);

                maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle * Mathf.Pow(ratio, 3), minSpeed, speed) * DateManager.timeScale;

                // Debug.Log($"{ratio}, {maneuverSpeed}");
            }


            t.rotation = Quaternion.SlerpUnclamped(t.rotation, targetRotation, Time.deltaTime * angularMultiplier * DateManager.timeScale);
            t.position = Vector3.SmoothDamp(t.position, t.position + t.forward * 20, ref ship.velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);
            return;
        }

        maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle, minSpeed, speed) * DateManager.timeScale;

        t.position = Vector3.SmoothDamp(t.position, dest, ref ship.velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);

        // Rotate much faster when close to destination
        t.rotation = Quaternion.SlerpUnclamped(t.rotation, targetRotation, Time.deltaTime * 2 * DateManager.timeScale);
    }

    void FightMovement()
    {
        // Simplified solution, if I think of something better I will change it

        if(CheckIfChangeDesination())
        {
            FindFightDestination();
        }
        FlyCapitan(destination.transform.position);
        for (int i = 0; i < composition.Count; i++)
        {
            FlyTowards(i);
        }
    }
    bool CheckIfChangeDesination()
    {
        if (Vector3.Distance(capitan.transform.position, destination.transform.position) < 10f) return true;
        if (Vector3.Distance(battleTarget.capitan.transform.position, destination.transform.position) > 45f) return true;
        return false;
    }
    public void FindFightDestination()
    {
        Vector3 dir = battleTarget.capitan.transform.position - capitan.transform.position;

        destination = point;
        destination.transform.position = capitan.transform.position + dir.normalized * Random.Range(25, 40);
    }
}
