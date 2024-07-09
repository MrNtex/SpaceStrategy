using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullding", menuName = "Economy/Bullding")]
public class Bullding : ScriptableObject
{
    public string bulldingName;
    public Sprite icon;

    public int timeToBuild;

    [System.Serializable]
    public struct MaterialYield
    {
        public MaterialType type;
        public int amount;
    }

    public List<MaterialYield> production;
    public List<MaterialYield> consumption;

    public int maxWorkers;

    public int energy; // Negative value means consumption, positive means production

    public List<BuildingPrerequisites> prerequisites; // Research will add building to the list of available buildings, do not change it here
}

public enum BuildingPrerequisites // Required planet characteristics to build a building
{
    HighEducation,
    RareEarths,
}