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
public class BodyStatus : MonoBehaviour
{
    public int population;
    public int maxPopulation;
    public float populationGrowthRate;
    public float populationGrowthRateModifier;

    public BodyStatusType status = BodyStatusType.Inhabitable;

    [Range(0, 100)]
    public float stability;

    [Range(0, 100)]
    public float hability;

}
