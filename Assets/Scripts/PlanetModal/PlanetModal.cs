using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    ColonyStatus colonyStatus;

    [SerializeField]
    private Graph graph;

    public CurrentGraph currentGraph;
    public void Spawn(BodyInfo bodyInfo)
    {

        // Set the planet modal's text to the bodyInfo's name
        planetName.text = bodyInfo.name;

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
        switch (currentGraph)
        {
            case CurrentGraph.Population:
                
                for (int i = 0; i < colonyStatus.recentPops.Count; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentPops[i]);
                }
                graph.GenerateAGraph(points, 12, 6);
                break;
            case CurrentGraph.GDP:

                for (int i = 0; i < colonyStatus.recentGDP.Count; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentGDP[i]);
                }
                graph.GenerateAGraph(points, 12, 6);
                break;
            case CurrentGraph.Stability:

                for (int i = 0; i < colonyStatus.recentStability.Count; i++)
                {
                    points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentStability[i]);
                }

                graph.GenerateAGraph(points, 12, 6);
                break;
        }
    }
}
