using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager instance;

    public List<Building> avaliableBuildings = new List<Building>();

    private void Awake()
    {
        instance = this;
    }
}
