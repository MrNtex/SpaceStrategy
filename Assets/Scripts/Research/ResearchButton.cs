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

    public Transform left, right;

    [Header("Line Renderer")]
    [SerializeField]
    private Material lineMaterial;
    private Color lineColor = Color.white;
    private float lineWidth = 0.1f;

    public ResearchManager.Research research;
    public void Create(ResearchManager.Research research)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(research.x * offset, research.y * offset);
        researchName.text = research.name;
        this.research = research;
    }
    public void CreateConnection(ResearchButton origin)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform);

        LineRenderer lr = line.AddComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.sortingOrder = 1;
        lr.useWorldSpace = false;

        if(origin.research.y == research.y)
        {
            // Simple straight line
            lr.positionCount = 2;
            lr.SetPosition(0, origin.right.position);
            lr.SetPosition(1, left.position);
            return;
        }
        lr.positionCount = 4;
        lr.SetPosition(0, origin.right.position);
        float middle = (origin.right.position.x + left.position.x) / 2;
        lr.SetPosition(1, new Vector3(middle, origin.right.position.y, left.position.z));
        lr.SetPosition(2, new Vector3(middle, left.position.y, left.position.z));
        lr.SetPosition(3, left.position);
    }
}
