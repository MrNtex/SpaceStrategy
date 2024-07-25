using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlanetModalInfrastructure : PlanetModalPage
{
    PlanetModal planetModal;
    ColonyStatus colonyStatus;

    [Header("Energy")]
    [SerializeField]
    private Graph graph;

    [SerializeField]
    private TMP_Text productionText, consumptionText;

    private const string unit = " EWh";
    [SerializeField]
    private Color consumptionWarning;

    [SerializeField]
    private Slider energySlider;

    [SerializeField]
    private Sprite[] powerPlants;
    [SerializeField]
    private Image powerPlantImage;
    [SerializeField]
    private TMP_Text powerPlantLevel;

    private Dictionary<int, string> numberToRoman = new Dictionary<int, string>
    {
        {1, "I"},
        {2, "II"},
        {3, "III"},
        {4, "IV"},
        {5, "V"},
        {6, "VI"},
        {7, "VII"},
        {8, "VIII"},
        {9, "IX"},
        {10, "X"}
    };
    public override void Create(PlanetModal planetModal)
    {
        this.planetModal = planetModal;
        colonyStatus = planetModal.colonyStatus;

        OnColonyUpdate();
    }
    public override void OnColonyUpdate()
    {
        DoGraph();
        UpdateEnergyText();

        powerPlantImage.sprite = powerPlants[colonyStatus.powerPlantLevel-1];
        powerPlantLevel.text = numberToRoman[colonyStatus.powerPlantLevel];
    }

    private void UpdateEnergyText()
    {
        productionText.text = colonyStatus.energyProduction.ToString("N2") + unit;

        if (colonyStatus.energyConsumption > colonyStatus.energyProduction)
        {
            consumptionText.color = consumptionWarning;
        }
        else
        {
            consumptionText.color = Color.white;
        }
        consumptionText.text = colonyStatus.energyConsumption.ToString("N2") + unit;
    }

    void DoGraph()
    {
        Dictionary<float, float> points = new Dictionary<float, float>();
        Dictionary<float, float> pointsConsumption = new Dictionary<float, float>();
        Dictionary<float, float> pointsDemand = new Dictionary<float, float>();
        int n = colonyStatus.recentEnergyProduction.Count;
        for (int i = 0; i < n; i++)
        {
            points.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentEnergyProduction[i]);
            pointsConsumption.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentEnergyConsumption[i]);
            pointsDemand.Add(DateManager.currentDate.Month - 5 + i, colonyStatus.recentEnergyDemand[i]);
        }
        List<Dictionary<float, float>> keyValuePairs = new List<Dictionary<float, float>>
        {
            points,
            pointsConsumption,
            pointsDemand
        };

        graph.GenerateAGraph(keyValuePairs, n, 6);
    }

    public void ChangeEnergyLevel()
    {
        colonyStatus.energyProductionMultiplier = energySlider.value;
        colonyStatus.CalculateConsuption();
        UpdateEnergyText();
    }
}
