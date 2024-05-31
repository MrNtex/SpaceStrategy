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

    private void Start()
    {
        // Debug
        Dictionary<float, float> test = new Dictionary<float, float>
        {
            { 0, 0 },
            { 1, 3 },
            { 2, 6 },
            { 3, 2 },
            { 4, 3 },
            { 5, 7 },
            { 6, 6 },
            { 7, 9 },
            { 8, 8 },
            { 9, 9 },
            { 10, 10 }
        };
        GenerateAGraph(test, 5, 5);
    }

    public void GenerateAGraph(Dictionary<float, float> points, int numberOfX, int numberOfY, bool sort = false)
    {
        if (sort)
        {
            points = new Dictionary<float, float>(points.OrderBy(x => x.Key));
        }

        float xMax = points.Keys.Max();
        float yMax = points.Values.Max();

        float xMin = points.Keys.Min();
        float yMin = points.Values.Min();

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
