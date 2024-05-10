using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResearchJSON : MonoBehaviour
{
    void Start()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("ResearchTrees");

        foreach (TextAsset file in jsonFiles)
        {
            Debug.Log($"Loaded file: {file.name}");

            // Parse each JSON file into a ResearchCategory object
            ResearchManager.ResearchCategory category = JsonUtility.FromJson<ResearchManager.ResearchCategory>(file.text);

            // Debug
            ProcessCategory(category);
        }
    }

    void ProcessCategory(ResearchManager.ResearchCategory category)
    {
        HashSet<int> ids = new HashSet<int>();
        Debug.Log($"Category Name: {category.name}");
        foreach (var research in category.researches)
        {
            if (ids.Contains(research.id))
            {
                Debug.LogError($"Duplicate ID found: {category.name} - {research.id}");
            }
            else
            {
                ids.Add(research.id);
            }
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
        public Research(int id, string name, string description, int cost, List<int> prerequisites, DateTime unlockedDate)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.cost = cost;
            this.prerequisites = prerequisites;
            researched = false;
            this.unlockedDate = unlockedDate;
        }
    }
}