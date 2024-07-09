using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    public List<ProcuctionMaterial> materials;

    public static Dictionary<MaterialType, ProcuctionMaterial> materialDict = new Dictionary<MaterialType, ProcuctionMaterial>();
    private void Awake()
    {
        foreach (ProcuctionMaterial m in materials)
        {
            materialDict.Add(m.type, m);
        }
    }
}
public enum MaterialType
{
    Metals,
    RareEarths,
    Ceramics,
    Chips,
    Food
}
[System.Serializable]
public class ProcuctionMaterial
{
    public MaterialType type;

    public string name;
    public Sprite icon;

    public int amount;
    int monthlyProduction;
}