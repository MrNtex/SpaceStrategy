using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTreeGenerator : MonoBehaviour
{
    int currentCategory = 0;

    [SerializeField]
    private GameObject researchPrefab;

    [SerializeField]
    private Transform researchParent;
    private void OnEnable()
    {
        foreach(var Research in ResearchJSON.Instance.categories[currentCategory].researches.Values)
        {
            GameObject go = Instantiate(researchPrefab, researchParent);
            go.GetComponent<ResearchButton>().Create(Research);
        }
    }
}