using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Graph : MonoBehaviour
{
    [SerializeField]
    private UIGridRenderer uiGridRenderer;

    [SerializeField]
    private GameObject lineRendererPrefab;

    [SerializeField]
    private RectTransform graphContainer;

    [SerializeField]
    private GameObject yMarker, xMarker;

    [SerializeField]
    private Color[] colors;

    public void GenerateAGraph(Dictionary<float, float> points, int numberOfX, int numberOfY, bool sort = false)
    {
        if(points.Count == 0)
        {
            return;
        }

        if (sort)
        {
            points = new Dictionary<float, float>(points.OrderBy(x => x.Key));
        }
        
        foreach(Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        float xMax = points.Keys.Max();
        float yMax = points.Values.Max();

        float xMin = points.Keys.Min();
        float yMin = points.Values.Min();

        float padding = (yMax-yMin) * 0.2f;
        if(padding < 0.1f)
        {
            padding = xMax * 0.2f;
        }

        yMax += padding;
        yMin -= padding;

        uiGridRenderer.gridSize = new Vector2Int(numberOfX, numberOfY);

        for (int i = 1; i < numberOfX; i++)
        {
            GameObject x = Instantiate(xMarker, graphContainer);
            x.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * graphContainer.sizeDelta.x / numberOfX, 0);
            x.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(xMin, xMax, i / (float)numberOfX).ToString("N1");
        }

        for(int i = 1; i < numberOfY; i++)
        {
            GameObject y = Instantiate(yMarker, graphContainer);
            y.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * graphContainer.sizeDelta.y / numberOfY);
            y.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(yMin, yMax, i / (float)numberOfY).ToString("N1");
        }

        UILineRenderer lineRenderer = Instantiate(lineRendererPrefab, graphContainer).GetComponent<UILineRenderer>();

        lineRenderer.points = new List<Vector2>();
        lineRenderer.color = colors[0];

        lineRenderer.uiGrid = GetComponent<UIGridRenderer>();


        foreach (KeyValuePair<float, float> point in points)
        {
            float x = Mathf.InverseLerp(xMin, xMax, point.Key) * numberOfX;
            float y = Mathf.InverseLerp(yMin, yMax, point.Value) * numberOfY;

            lineRenderer.points.Add(new Vector2(x, y));

        }
    }
    // Multiple lines
    public void GenerateAGraph(List<Dictionary<float, float>> points, int numberOfX, int numberOfY)
    {
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        float xMax = points[0].Keys.Max();
        float yMax = points[0].Values.Max();

        float xMin = points[0].Keys.Min();
        float yMin = points[0].Values.Min();

        float padding = (yMax - yMin) * 0.2f;

        yMax += padding;
        yMin -= padding;

        uiGridRenderer.gridSize = new Vector2Int(numberOfX, numberOfY);

        for (int i = 1; i < numberOfX; i++)
        {
            GameObject x = Instantiate(xMarker, graphContainer);
            x.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * graphContainer.sizeDelta.x / numberOfX, 0);
            x.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(xMin, xMax, i / (float)numberOfX).ToString();
        }

        for (int i = 1; i < numberOfY; i++)
        {
            GameObject y = Instantiate(yMarker, graphContainer);
            y.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * graphContainer.sizeDelta.y / numberOfY);
            y.GetComponentInChildren<TMPro.TMP_Text>().text = Mathf.Lerp(yMin, yMax, i / (float)numberOfY).ToString();
        }
        int colorIndex = 0;
        foreach(Dictionary<float, float> tuple in points)
        {
            UILineRenderer lineRenderer = Instantiate(lineRendererPrefab, graphContainer).GetComponent<UILineRenderer>();

            lineRenderer.points = new List<Vector2>();
            lineRenderer.color = colors[colorIndex];
            colorIndex++;

            lineRenderer.uiGrid = GetComponent<UIGridRenderer>();
            

            foreach (KeyValuePair<float, float> point in tuple)
            {
                float x = Mathf.InverseLerp(xMin, xMax, point.Key) * numberOfX;
                float y = Mathf.InverseLerp(yMin, yMax, point.Value) * numberOfY;

                lineRenderer.points.Add(new Vector2(x,y));

            }
        }
        
    }
}
