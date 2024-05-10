using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResearchButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private TMP_Text researchName;

    private const float offset = 100;
    public void Create(ResearchManager.Research research)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(research.x * offset, research.y * offset);
        researchName.text = research.name;
    }
}
