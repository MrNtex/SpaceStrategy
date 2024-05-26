using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTreeGenerator : MonoBehaviour
{
    public int currentCategory = 0;

    [SerializeField]
    private GameObject researchPrefab;

    [SerializeField]
    private Transform researchParent;

    List<ResearchButton> researchButtons = new List<ResearchButton>();
    ResearchCategory[] rCategory;

    public List<CategoryPanel> categoryPanels = new List<CategoryPanel>();

    [SerializeField]
    private GameObject categoryPrefab;
    private void Start()
    {
        ResearchCategory[] rCategory = ResearchJSON.Instance.categories.ToArray();

        for(int i = 0; i < rCategory.Length; i++)
        {
            GenerateTree(rCategory[i]);
        }


    }
    void GenerateTree(ResearchCategory category)
    {
        GameObject categoryGO = Instantiate(categoryPrefab, researchParent);
        CategoryPanel categoryPanel = categoryGO.GetComponent<CategoryPanel>();
        categoryPanels.Add(categoryPanel);
        
        foreach (var Research in category.researches)
        {
            
            GameObject go = Instantiate(researchPrefab, categoryGO.transform);
            
            ResearchButton researchButton = go.GetComponent<ResearchButton>();
            researchButton.Create(Research);
            categoryPanel.researchButtons.Add(Research.id, researchButton);
            
            foreach (int prereq in Research.prerequisites)
            {
                if (categoryPanel.researchButtons.ContainsKey(prereq))
                {
                    researchButton.CreateConnection(categoryPanel.researchButtons[prereq]);
                }
                else
                {
                    Debug.LogError($"Research {Research.name} has a prerequisite that does not exist or hasn't yet been initialized: {prereq}");
                }
            }
        }
    }
}