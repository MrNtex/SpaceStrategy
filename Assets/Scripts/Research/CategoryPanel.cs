using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  // Needed for UI event handling
using UnityEngine.UI;           // For using Image

public class CategoryPanel : MonoBehaviour
{
    public ResearchCategory category;
    public Dictionary<int, ResearchButton> researchButtons = new Dictionary<int, ResearchButton>();
}
