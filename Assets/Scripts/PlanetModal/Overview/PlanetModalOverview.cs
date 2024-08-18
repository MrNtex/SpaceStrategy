using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanetModalOverview : PlanetModalPage, IPieCharDataTarget
{
    private PlanetModal planetModal;
    private ColonyStatus colonyStatus;

    [SerializeField]
    private Graph graph;

    public CurrentGraph currentGraph;

    [SerializeField]
    private Color selectedColor, defaultColor;
    private int currentGraphIndex = 0;
    [SerializeField]
    private Image[] graphButtons;


    [SerializeField]
    private TMP_Text hability, stability, population;

    public override void Create(PlanetModal planetModal)
    {
        this.planetModal = planetModal;
        colonyStatus = planetModal.colonyStatus;
        SetCurrentGraph(0);
    }

    public override void OnColonyUpdate()
    {
        hability.text = colonyStatus.hability.ToString() + "%";
        stability.text = colonyStatus.stability.ToString() + "%";
        population.text = colonyStatus.population.ToString();

        DoGraph();
    }

    public void SetCurrentGraph(int currentGraph)
    {
        if (this.currentGraphIndex == currentGraph)
        {
            return;
        }

        graphButtons[this.currentGraphIndex].color = defaultColor;
        graphButtons[currentGraph].color = selectedColor;
        this.currentGraphIndex = currentGraph;

        this.currentGraph = (CurrentGraph)currentGraph;
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

    public TooltipData GetTooltipData(int slice)
    {
        throw new System.NotImplementedException();
    }
}
