using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class FriendlyFleet : Fleet
{
    public override void SetDestination(GameObject dest)
    {
        base.SetDestination(dest);
        SetFleetStatus(FleetStatus.Moving);
    }

    public override void SetFleetStatus(FleetStatus status)
    {
        this.status = status;

        if(status != FleetStatus.Idle) onOrbit = null;

        switch (status)
        {
            case FleetStatus.Idle:
                DrawPath(capitan.transform.position); // Clear path
                break;
            default:
                break;
        }

        BodyInfoUI.instance.SetBody(this, false);
    }
    public override void ButtonClicked()
    {
        objectFocusHelper.cameraPlacement.SetParent(capitan.transform);
        BodyInfoUI.instance.SetBody(this);

        cameraFocus.FocusOn(objectFocusHelper, FleetManager.instance.selectedFleet == this);

        FleetManager.instance.selectedFleet = this; // Reminder: Selected Object and Selected Fleet must be different (they are changed under similar yet diffrent circumstances)
    }
    public override void DrawPath(Vector3 dest)
    {
        if (status == FleetStatus.Moving)
        {
            path.positionCount = 2;
            path.SetPosition(0, capitan.transform.position + capitan.transform.forward * lrOffset);
            path.SetPosition(1, dest);
        }
        else
        {
            path.positionCount = 0;
        }
    }

    public override void SetStatus(ref TMP_Text text)
    {
        text.color = ColorManager.instance.fleetDestination;
        switch (status)
        {
            case FleetStatus.Idle:
                if (onOrbit)
                {
                    text.text = $"Orbiting <link=\"CelestialBody\"><color=#ffd666>{onOrbit.name}</color></link>";
                    break;
                }
                text.text = "Idle";
                break;
            case FleetStatus.Moving:
                if(destination.CompareTag("CelestialBody"))
                {
                    text.text = $"Moving to <link=\"CelestialBody\"><color=#ffd666>{destination.name}</color></link>";
                    ClickableLinkHandler.adress = destination;
                }
                else
                {
                    string point = "X: " + destination.transform.position.x + " Y: " + destination.transform.position.z; // Y because of the 2D plane
                    text.text = "Moving to " + point;
                }
                break;
            default:
                text.text = "No status";
                break;
        }
    }
}
