using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullding", menuName = "Economy/Bullding")]
public class Building : ScriptableObject
{
    public string buildingName;
    public Sprite icon;
    public Sprite simpleIcon;
    public Sprite background;


    public int constructionTime;

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

    public bool unique; // Only one building of this type can be built on a planet
}

public enum BuildingPrerequisites // Required planet characteristics to build a building
{
    HighEducation,
    RareEarths,
}