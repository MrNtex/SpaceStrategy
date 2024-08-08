using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class FriendlyFleet : Fleet
{


    protected override void Start()
    {
        base.Start();
        effects = new List<LeftPanelEffect>()
        {
            new LeftPanelButton(
                "Halt",
                "Immediately stop all movement [H]",
                FleetManager.instance.haltFleet, // Action to halt the fleet
                () => SetFleetStatus(FleetStatus.Idle) // Action to set fleet status to Idle
            ),
        };
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (status == FleetStatus.Moving && destinationInfo is EnemyFleet)
        {
            EnemyFleet targetFleet = destinationInfo as EnemyFleet;
            float distance = Vector3.Distance(capitan.transform.position, targetFleet.capitan.transform.position);

            if(distance < fightingRange)
            {
                
                BattlesManager.instance.AddBattle(this, targetFleet);
            }
            
        }
    }

    public override void SetDestination(GameObject dest)
    {
        base.SetDestination(dest);
        SetFleetStatus(FleetStatus.Moving);
    }

    public override void UpdateFleet(bool billboard = true)
    {
        base.UpdateFleet(billboard);
        if(composition.Count == 0)
        {
            FleetManager.instance.fleets.Remove(this);
            return;
        }
    }

    public override void SetFleetStatus(FleetStatus status)
    {
        base.SetFleetStatus(status);

        if(status != FleetStatus.Idle) onOrbit = null;

        DrawPath(capitan.transform.position); // Clear path

        BodyInfoUI.instance.SetBody(this, false);
    }
    public override void ButtonClicked()
    {
        base.ButtonClicked();

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
                text.text = StatusMoving();
                break;
            default:
                text.text = "No status";
                break;
        }
    }
    private string StatusMoving()
    {
        string text = "";
        if (destination.CompareTag("CelestialBody"))
        {
            text = $"Moving to <link=\"CelestialBody\"><color=#ffd666>{destination.name}</color></link>";
            ClickableLinkHandler.adress = destination;
        }
        else if (destination.CompareTag("Ship"))
        {

            Fleet fleet = destination.GetComponentInParent<Fleet>();
            //ENEMY FLEET
            if (fleet is EnemyFleet)
            {
                text = $"Engaging <link=\"Fleet\"><color=#ffd666>{fleet.name}</color></link>";
            }
            else
            {
                text = $"Following <link=\"Fleet\"><color=#ffd666>{fleet.name}</color></link>";
            }
            ClickableLinkHandler.adress = destination.transform.parent.gameObject;
        }
        else
        {
            string point = "X: " + destination.transform.position.x + " Y: " + destination.transform.position.z; // Y because of the 2D plane
            text = "Moving to " + point;
        }

        return text;
    }
}
