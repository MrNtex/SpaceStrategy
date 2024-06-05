using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchUI : MonoBehaviour
{
    public static ResearchUI instance;

    [SerializeField]
    private ResearchTreeGenerator researchTreeGenerator;

    [SerializeField]
    private ResearchPanelMovement researchPanelMovement;

    ResearchCategory[] rCategory;

    [SerializeField]
    private GameObject topbar, cattegoryButton;

    [SerializeField]
    private ToggleGroup toggleGroup;

    public int currentCategory = 0;

    public List<GameObject> categoryPanels = new List<GameObject>();

    public void Config(ResearchCategory[] rCategory)
    {
        gameObject.SetActive(true); // I have to set it active, because otherwise the line renderer won't work, because positions don't update

        rCategory = ResearchManager.instance.categories.ToArray();

        for (int i = 0; i < rCategory.Length; i++)
        {
            categoryPanels.Add(researchTreeGenerator.GenerateTree(rCategory[i]));
            GameObject go = Instantiate(cattegoryButton, topbar.transform);
            go.GetComponentInChildren<TMP_Text>().text = rCategory[i].name;
            go.GetComponent<Toggle>().group = toggleGroup;
            go.GetComponent<Toggle>().onValueChanged.AddListener((value) => ChangeCategory(i-1));
        }

        categoryPanels[currentCategory].SetActive(true);

        gameObject.SetActive(false);
    }

    void ChangeCategory(int category)
    {
        categoryPanels[currentCategory].SetActive(false);
        currentCategory = category;
        categoryPanels[category].SetActive(true);
        researchPanelMovement.ChangeCategory();
    }
}
