using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryPanel : MonoBehaviour
{
    public ResearchManager.ResearchCategory category;

    public Dictionary<int, ResearchButton> researchButtons = new Dictionary<int, ResearchButton>();
}
