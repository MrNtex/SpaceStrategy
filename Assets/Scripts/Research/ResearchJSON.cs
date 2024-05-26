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
            ResearchCategory category = JsonUtility.FromJson<ResearchCategory>(file.text);

            // Debug
            categories.Add(category);
        }
    }
}