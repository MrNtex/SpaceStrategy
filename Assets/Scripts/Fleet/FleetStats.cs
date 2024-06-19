using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FleetStats
{
    public Fleet myFleet;

    public List<Ship> screens = new List<Ship>();
    public List<Ship> capitals = new List<Ship>();

    public float lightDamage;
    public float heavyDamage;

    public int chanceOfHittingCapitals = 1;

    TargetSelection targetSelection = TargetSelection.Random; // For now, we will use random selection

    public void ShipDestroyed(Ship target)
    {
        if (CheckIfScreen(target))
        {
            screens.Remove(target);
        }
        else
        {
            capitals.Remove(target);
        }
        myFleet.RemoveFromFleet(target);
        GetCapitalsChance();
    }

    public void GetCapitalsChance()
    {
        if (capitals.Count > 0)
        {
            float chanceOfHittingCapitalsDyn = 4 * screens.Count / capitals.Count; // The should be at least 4 screens per capital
            if (chanceOfHittingCapitalsDyn < 1)
            {
                chanceOfHittingCapitals = 0; // If the condition is met, we don't care about capitals
                return;
            }
            chanceOfHittingCapitals = (int)Mathf.Ceil(Mathf.Pow(chanceOfHittingCapitalsDyn, 2)); // Converted to int, because some floats won't fit into int
            return;
        }

        chanceOfHittingCapitals = 1; // If there are no screens, we are forced to hit capitals
    }
    public static bool CheckIfScreen(Ship s)
    {
        return s.type == ShipType.Fighter || s.type == ShipType.Destroyer || s.type == ShipType.LightCruiser;
    }
}
