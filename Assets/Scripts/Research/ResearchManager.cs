using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public static ResearchManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public struct ResearchCategory
{
    public string name;
    public List<Research> researches;
}
[Serializable]
public struct Research
{
    public int id;
    public string name;
    public string description;
    public int cost;

    public List<int> prerequisites;

    public bool researched;

    public DateTime unlockedDate;

    public float x, y;
    public Research(int id, string name, string description, int cost, List<int> prerequisites, DateTime unlockedDate, float x, float y)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.prerequisites = prerequisites;
        researched = false;
        this.unlockedDate = unlockedDate;

        this.x = x;
        this.y = y;
    }
}
