# Fleet Battle System

This file briefly describes the combat system used in a game. The system includes mechanics for initializing fleets, attacking, and reducing damage based on armor. It also incorporates logic to make hitting capital ships less likely based on the number of screen ships.

## Classes and Enums

### `Battle`
The main class handles the battle mechanics between two fleets.

#### Fields
- `friendlyFleet`: Instance of the friendly fleet.
- `enemyFleet`: Instance of the enemy fleet.
- `armorScalingFactor`: A constant used to scale the damage reduction based on armor (default is 0.1).
- `friendlyFleetStats`: Statistics of the friendly fleet.
- `enemyFleetStats`: Statistics of the enemy fleet.

#### Methods
- `Initialize(FriendlyFleet ff, EnemyFleet ef)`: Initializes the fleets and generates their statistics.
- `GenerateFleetStats(Fleet fleet)`: Generates statistics for the given fleet.
- `BattleTick()`: Executes a tick of the battle where both fleets attack each other. Should be invoked by the date manager.
- `Attack(ref FleetStats source, ref FleetStats target)`: Handles the attack from one fleet to another.
- `GetTarget(ref FleetStats source, ref FleetStats target)`: Selects a target ship based on the chance of hitting capital ships.
- `CalculateDamage(Ship target, ref FleetStats stats)`: Calculates the damage inflicted on the target ship considering its armor.

### `FleetStats`
A class representing the statistics of a fleet.

#### Fields
- `myFleet`: The fleet instance.
- `screens`: List of screen ships.
- `capitals`: List of capital ships.
- `lightDamage`: Total light damage of the fleet.
- `heavyDamage`: Total heavy damage of the fleet.
- `chanceOfHittingCapitals`: The calculated chance of hitting capital ships.
- `targetSelection`: Enum value to determine the target selection strategy.

#### Methods
- `ShipDestroyed(Ship target)`: Removes a destroyed ship from the fleet and updates the chance of hitting capital ships.
- `GetCapitalsChance()`: Calculates the chance of hitting capital ships based on the number of screen ships.
- `CheckIfScreen(Ship s)`: Checks if a ship is a screen ship.

### `TargetSelection`
Enum for different target selection strategies.
- `Random`
- `Divide`
- `Weakest`
- `Strongest`

## Damage Reduction Formula

The formula to reduce the damage based on armor is defined as follows (only applies for light damage):
```math
\text{Effective Damage} = \text{Base Damage} \times \frac{1}{1 + k \times \text{Armor}}
```
Where:
- `Effective Damage`: The damage after reduction.
- `Base Damage`: The initial damage before reduction.
- `Armor`: The armor value of the unit.
- `k`: A scaling factor (default is 0.1).

### Example Calculation

```csharp
float CalculateDamage(Ship target, ref FleetStats stats)
{
    float dmg = 0;

    float lightDamageInfluence = 1 + target.stats.armor * armorScalingFactor;
    dmg += stats.lightDamage / lightDamageInfluence;

    dmg += stats.heavyDamage; // Heavy damage is always applied, there is no armor scaling

    return dmg;
}
```
Hard damage is usually smaller, but it's completely ignored by the armor.

### Target Selection

The selection system forces players, to make their fleet composition more considered, by using both screens and capitals shipt.
#### Process

##### Determine Screens and Capitals:
The fleet is divided into screens (smaller, protective ships) and capitals (larger, more valuable ships).

##### Calculate the Chance of Hitting Capital Ships:
The chance of hitting a capital ship increases if there are fewer screen ships relative to capital ships. This is calculated using:

```csharp

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
```
#### Select Target:

 A random seed is generated to decide whether a screen or a capital ship is targeted. If the seed falls within the range of screen ships, a random screen ship is targeted; otherwise, a random capital ship is targeted.

``` csharp

private Ship GetTarget(ref FleetStats source, ref FleetStats target)
{
    int screensCount = target.screens.Count;
    int seed = Random.Range(0, screensCount + target.chanceOfHittingCapitals);

    if (seed > screensCount)
    {
        // Hit random capital
        return target.capitals[Random.Range(0, target.capitals.Count)];
    }
    // Hit random screen
    return target.screens[Random.Range(0, screensCount)];
}
```
