using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResearchJSON : MonoBehaviour
{
    public static ResearchJSON Instance { get; private set; }

    

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
            ResearchCategoryJSON category = JsonUtility.FromJson<ResearchCategoryJSON>(file.text);

            // Debug
            ResearchCategory formatedCategory = new ResearchCategory();

            formatedCategory.name = category.name;

            formatedCategory.researches = new Dictionary<int, Research>();

            foreach (ResearchJSONObject research in category.researches)
            {
                Research formatedResearch = new Research(research.id, research.name, research.description, research.duration, research.prerequisites, research.effects.ToArray(), research.x, research.y);
                formatedCategory.researches.Add(research.id, formatedResearch);
            }

            ResearchManager.instance.categories.Add(formatedCategory);
        }
    }
}
[Serializable]
public struct ResearchCategoryJSON
{
    public string name;
    public List<ResearchJSONObject> researches;
}
[Serializable]
public struct ResearchJSONObject
{
    public int id;
    public string name;
    public string description;
    public int duration;

    public List<int> prerequisites;

    public List<string> effects;

    public bool researched;


    public float x, y;
    public ResearchJSONObject(int id, string name, string description, int duration, List<string> effects, List<int> prerequisites, float x, float y)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.duration = duration;
        this.prerequisites = prerequisites;
        this.effects = effects;
        researched = false;

        this.x = x;
        this.y = y;
    }
}