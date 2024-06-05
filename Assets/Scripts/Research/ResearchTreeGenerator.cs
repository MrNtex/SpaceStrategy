using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTreeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject researchPrefab;

    [SerializeField]
    private Transform researchParent;

    List<ResearchButton> researchButtons = new List<ResearchButton>();


    public List<CategoryPanel> categoryPanels = new List<CategoryPanel>();

    [SerializeField]
    private GameObject categoryPrefab;
    public GameObject GenerateTree(ResearchCategory category)
    {
        GameObject categoryGO = Instantiate(categoryPrefab, researchParent);
        CategoryPanel categoryPanel = categoryGO.GetComponent<CategoryPanel>();
        categoryPanels.Add(categoryPanel);
        
        foreach (var Research in category.researches)
        {
            
            GameObject go = Instantiate(researchPrefab, categoryGO.transform);
            
            ResearchButton researchButton = go.GetComponent<ResearchButton>();
            researchButton.Create(Research.Value);
            categoryPanel.researchButtons.Add(Research.Key, researchButton);
            
            foreach (int prereq in Research.Value.prerequisites)
            {
                if (categoryPanel.researchButtons.ContainsKey(prereq))
                {
                    researchButton.CreateConnection(categoryPanel.researchButtons[prereq]);
                }
                else
                {
                    Debug.LogError($"Research {Research.Value.name} has a prerequisite that does not exist or hasn't yet been initialized: {prereq}");
                }
            }
        }

        return categoryGO;
    }
}
