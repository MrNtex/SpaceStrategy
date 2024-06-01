using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BodyStatusType
{
    Colonized,
    Colonizable,
    CanBeTerraformed,
    Specialized, // For bodies that can't be colonized but can have scientific outposts or mining operations
    CanBeSpecialized,
    Inhabitable
}
public class ColonyStatus : MonoBehaviour
{
    public int population;
    public int maxPopulation;
    public float populationGrowthRate;
    public float populationGrowthRateModifier = 1;


    [Range(0, 100)]
    public float stability;

    [Range(0, 100)]
    public float hability;


    CircularBuffer<int> recentPops = new CircularBuffer<int>(5);

    void Start()
    {
        recentPops.Add(population);
    }
    public void UpdateColony()
    {
        population += (int)(population * populationGrowthRate * populationGrowthRateModifier);
        recentPops.Add(population);
    }
}
