using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ResearchManager;

public class ResearchJSON : MonoBehaviour
{
    public static ResearchJSON Instance { get; private set; }

    public List<ResearchCategory> categories;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("ResearchTrees");

        foreach (TextAsset file in jsonFiles)
        {
            Debug.Log($"Loaded file: {file.name}");

            // Parse each JSON file into a ResearchCategory object
            ResearchManager.ResearchCategory category = JsonUtility.FromJson<ResearchManager.ResearchCategory>(file.text);

            // Debug
            categories.Add(category);
        }
    }
}
public class ResearchManager
{
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
}