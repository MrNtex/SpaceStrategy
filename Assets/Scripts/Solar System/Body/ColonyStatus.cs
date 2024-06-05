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
    public float population;
    public int maxPopulation;
    public float populationGrowthRate;


    [Range(0, 100)]
    public float stability;

    [Range(0, 100)]
    public float hability;

    public float gdp = 1010.3f; // In trillions
    public float gdpChangeRate = 0.02f;


    public CircularBuffer<float> recentPops = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentGDP = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentStability = new CircularBuffer<float>(12);
    void Start()
    {
        recentPops.Add(population);
    }
    public void UpdateColony()
    {
        population += (int)(population * (populationGrowthRate + Random.Range(-.5f,.5f)));
        recentPops.Add(population);

        recentStability.Add(stability);


        // Scaling by pops, stability, 
        gdp += gdp * gdpChangeRate * Random.Range(-.5f, .5f);
        recentGDP.Add(gdp);
    }
}
