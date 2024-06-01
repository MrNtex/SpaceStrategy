using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private RectTransform graphContainer;

    [SerializeField]
    private GameObject yMarker, xMarker;

    public void GenerateAGraph(Dictionary<float, float> points, int numberOfX, int numberOfY, bool sort = false)
    {
        if (sort)
        {
            points = new Dictionary<float, float>(points.OrderBy(x => x.Key));
        }
        Transform l = lineRenderer.transform;
        foreach (Transform child in graphContainer)
        {
            if (child != l)
                Destroy(child.gameObject);
        }

        float xMax = points.Keys.Max();
        float yMax = points.Values.Max();

        float xMin = points.Keys.Min();
        float yMin = points.Values.Min();

        float padding = (yMax-yMin) * 0.2f;

        yMax += padding;
        yMin -= padding;
        for(int i = 1; i < numberOfX; i++)
        {
            GameObject x = Instantiate(xMarker, graphContainer);
            x.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * graphContainer.sizeDelta.x / numberOfX, 0);
            x.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(xMin, xMax, i / (float)numberOfX).ToString();
        }

        for(int i = 1; i < numberOfY; i++)
        {
            GameObject y = Instantiate(yMarker, graphContainer);
            y.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * graphContainer.sizeDelta.y / numberOfY);
            y.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(yMin, yMax, i / (float)numberOfY).ToString();
        }

        lineRenderer.positionCount = 0;

        foreach(KeyValuePair<float, float> point in points)
        {
            lineRenderer.positionCount++;

            float x = Mathf.InverseLerp(xMin, xMax, point.Key) * graphContainer.sizeDelta.x;
            float y = Mathf.InverseLerp(yMin, yMax, point.Value) * graphContainer.sizeDelta.y;

            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(x, y, 0));
            
        }
    }
}
