using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public static ResearchManager instance;

    public List<ResearchCategory> categories;

    public List<ActiveResearch> activeResearches = new List<ActiveResearch>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DateManager.instance.OnDateUpdate += HandleDateChanged;
    }

    public void StartResearch(ResearchButton button)
    {
        ActiveResearch activeResearch = new ActiveResearch
        {
            research = button.research,
            startDate = DateManager.currentDate,
            button = button
        };
        activeResearches.Add(activeResearch);
    }
    // Update is called once per frame
    void HandleDateChanged()
    {
        foreach (ActiveResearch research in activeResearches)
        {
            float progress = (float)DateManager.currentDate.Subtract(research.startDate).TotalDays / research.research.duration;

            if (progress >= 1)
            {
                research.research.researched = true;
                research.research.unlockedDate = DateManager.currentDate;
                //activeResearches.Remove(research);
            }
        }
    }
}
public struct ActiveResearch
{
    public Research research;
    public DateTime startDate;
    public ResearchButton button;
}

[Serializable]
public struct ResearchCategory
{
    public string name;
    public Dictionary<int, Research> researches;
}
[Serializable]
public class Research
{
    public int id;
    public string name;
    public string description;
    public int duration;

    public List<int> prerequisites;

    public bool researched;

    public DateTime unlockedDate;

    public Dictionary<string, int> effects; // Tag, Value

    public float x, y;
    public Research(int id, string name, string description, int duration, List<int> prerequisites, string[] effects, float x, float y)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.duration = duration;
        this.prerequisites = prerequisites;
        researched = false;

        this.effects = JSONUtils.StringIntToDictionary(effects);

        this.x = x;
        this.y = y;
    }
}
