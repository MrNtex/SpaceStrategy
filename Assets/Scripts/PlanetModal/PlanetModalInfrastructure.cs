using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetModalInfrastructure : PlanetModalPage
{
    PlanetModal planetModal;
    ColonyStatus colonyStatus;

    [SerializeField]
    private Graph graph;

    public override void Create(PlanetModal planetModal)
    {
        this.planetModal = planetModal;
        colonyStatus = planetModal.colonyStatus;

        DoGraph();
    }
    public override void OnColonyUpdate()
    {
        DoGraph();
    }

    void DoGraph()
    {
        Dictionary<float, float> points = new Dictionary<float, float>();
        Dictionary<float, float> pointsConsumption = new Dictionary<float, float>();
        int n = colonyStatus.recentEnergyProduction.Count;
        for (int i = 0; i < n; i++)
        {
            points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentEnergyProduction[i]);
            pointsConsumption.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentEnergyConsumption[i]);
        }
        List<Dictionary<float, float>> keyValuePairs = new List<Dictionary<float, float>>();
        keyValuePairs.Add(points);
        keyValuePairs.Add(pointsConsumption);

        graph.GenerateAGraph(keyValuePairs, n, 6);
    }
}
