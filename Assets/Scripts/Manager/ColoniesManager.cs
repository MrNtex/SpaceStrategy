using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoniesManager : MonoBehaviour
{
    public static ColoniesManager instance;

    public List<ColonyStatus> colonies = new List<ColonyStatus>();

    public float constructionSpeed = 1;
    public float construcitonEnergyCost = 1;

    public Dictionary<int, float> energyProductionByLevel = new Dictionary<int, float>
    {
        {1, 1},
        {2, 2},
        {3, 3},
        {4, 4},
        {5, 5},
        {6, 6},
        {7, 7},
        {8, 8},
        {9, 9},
        {10, 10},
    };
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple ColoniesManagers in scene");
            Destroy(gameObject);
        }
    }
}
