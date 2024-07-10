using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentGraph
{
    Population,
    GDP,
    Stability,
}
public class PlanetModal : MonoBehaviour
{

    [SerializeField]
    private GameObject planetModal;

    [SerializeField]
    private TMP_Text planetName;

    [SerializeField]
    private TMP_Text hability, stability, population;

    public BodyInfo bodyInfo;
    public ColonyStatus colonyStatus;

    [SerializeField]
    private Graph graph;

    public CurrentGraph currentGraph;

    [SerializeField]
    private Color selectedColor, defaultColor;
    private int currentGraphIndex = 0;
    [SerializeField]
    private Image[] graphButtons;
    [SerializeField]
    private Image topBar;
    public void Spawn(BodyInfo bodyInfo)
    {

        // Set the planet modal's text to the bodyInfo's name
        planetName.text = bodyInfo.name;

        topBar.sprite = bodyInfo.background;

        colonyStatus = bodyInfo.colonyStatus;
        if (colonyStatus == null)
        {
            Debug.LogError("colonyStatus is null");
            return;
        }

        this.bodyInfo = bodyInfo;

        OnColonyUpdate();

        ColoniesManager.instance.OnColonyUpdate += OnColonyUpdate;
    }
    public void SetCurrentGraph(int currentGraph)
    {
        if(this.currentGraphIndex == currentGraph)
        {
            return;
        }

        graphButtons[this.currentGraphIndex].color = defaultColor;
        graphButtons[currentGraph].color = selectedColor;
        this.currentGraphIndex = currentGraph;

        this.currentGraph = (CurrentGraph)currentGraph;
        DoGraph();
    }
    public void DestroySelf()
    {
        MenusManager.activeModals.Remove(gameObject);
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        ColoniesManager.instance.OnColonyUpdate -= OnColonyUpdate;
    }
    void OnColonyUpdate()
    {;
        // UpdateVariables

        hability.text = colonyStatus.hability.ToString() + "%";
        stability.text = colonyStatus.stability.ToString() + "%";
        population.text = colonyStatus.population.ToString();

        DoGraph();
    }
    void DoGraph()
    {
        Dictionary<float, float> points = new Dictionary<float, float>();
        int n = 0;
        switch (currentGraph)
        {
            case CurrentGraph.Population:
                n = colonyStatus.recentPops.Count;
                for (int i = 0; i < n; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentPops[i]);
                }
                graph.GenerateAGraph(points, n, 6, false, true);
                break;
            case CurrentGraph.GDP:
                n = colonyStatus.recentGDP.Count;
                for (int i = 0; i < n; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentGDP[i]);
                }
                graph.GenerateAGraph(points, n, 6, false, true);
                break;
            case CurrentGraph.Stability:
                n = colonyStatus.recentStability.Count;
                for (int i = 0; i < n; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentStability[i]);
                }

                graph.GenerateAGraph(points, n, 6, false, true);
                break;
        }
    }
}
