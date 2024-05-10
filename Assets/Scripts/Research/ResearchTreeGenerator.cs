using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTreeGenerator : MonoBehaviour
{
    int currentCategory = 0;
    private void OnEnable()
    {
        foreach(var Research in ResearchJSON.Instance.categories[currentCategory].researches)
        {
            Debug.Log(Research.Value.name);
        }
    }
}