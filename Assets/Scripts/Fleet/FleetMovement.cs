using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fleet : ObjectInfo
{

    private Vector3 capitanVelocity = Vector3.zero;

    private const float forceManueverDistance = 50f;


    private void FixedUpdate()
    {
        if (status == FleetStatus.Moving && destination != null)
        {
            if (destination.CompareTag("Point"))
            {
                FlyTowards(destination.transform.position);
                return;
            }

            Vector3 near = destination.transform.position - capitan.transform.position;
            near -= near.normalized;
            near += capitan.transform.position;
            FlyTowards(near);

        }
    }
    void FlyTowards(Vector3 dest)
    {
        if (Vector3.Distance(capitan.transform.position, dest) < destinationOffset)
        {
            if (destination.CompareTag("CelestialBody"))
            {
                gameObject.transform.SetParent(destination.transform);
                onOrbit = destination;
            }
            SetFleetStatus(FleetStatus.Idle);
            return;
        }

        DrawPath(dest);
        CalculateMovment(dest, capitan.transform, ref capitanVelocity);
    }

    public void CalculateMovment(Vector3 dest, Transform ship, ref Vector3 velocity)
    {
        if (DateManager.timeScale == 0) return;

        if (DateManager.timeScale > 50)
        {
            ship.position = Vector3.MoveTowards(ship.position, dest, speed * DateManager.timeScale * Time.deltaTime);
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(dest - ship.position);

        float distance = Vector3.Distance(ship.position, dest);

        Vector3 dir = dest - ship.position;
        float angle = Vector3.Angle(ship.forward, dir); // The resulting angle ranges from 0 to 180.

        float maneuverSpeed = speed * DateManager.timeScale;
        Debug.Log(maneuverSpeed);

        if (distance > forceManueverDistance && !forceManeuver)
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


            ship.rotation = Quaternion.SlerpUnclamped(ship.rotation, targetRotation, Time.deltaTime * angularMultiplier * DateManager.timeScale);
            ship.position = Vector3.SmoothDamp(ship.position, ship.position + ship.forward * 20, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);
            return;
        }

        maneuverSpeed = Mathf.Clamp(speed - maneuverabilityPenalty * angle, minSpeed, speed) * DateManager.timeScale;

        ship.position = Vector3.SmoothDamp(ship.position, dest, ref velocity, smoothTime, maneuverSpeed, Time.deltaTime * DateManager.timeScale);

        // Rotate much faster when close to destination
        ship.rotation = Quaternion.SlerpUnclamped(ship.rotation, targetRotation, Time.deltaTime * 2 * DateManager.timeScale);
    }
}
