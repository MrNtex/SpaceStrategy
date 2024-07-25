using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum BodyStatusType
{
    Colonized,
    Colonizable,
    CanBeTerraformed,
    Specialized, // For bodies that can't be colonized but can have scientific outposts or mining operations
    CanBeSpecialized,
    Inhabitable
}
public class ColonyStatus : MonoBehaviour
{
    public float population;
    public int maxPopulation;
    public float populationGrowthRate;

    public delegate void ColonyUpdate();
    public event ColonyUpdate OnColonyUpdate;

    [Range(0, 100)]
    public float stability;

    [Range(0, 100)]
    public float hability;

    public float gdp = 1010.3f; // In trillions
    public float gdpChangeRate = 0.02f;


    public CircularBuffer<float> recentPops = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentGDP = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentStability = new CircularBuffer<float>(12);

    [Header("Buildings")]
    public const int maxBuildings = 12;
    public int avaliableSlots = 5;

    public class PlacedBuilding
    {
        public Building building;
        public bool active;
        public bool notEnoughEnergy;

        public PlacedBuilding(Building building)
        {
            this.building = building;
            this.active = true;
        }
    }
    public List<PlacedBuilding> buildings = new List<PlacedBuilding>();

    public Queue<Construction> constructionQueue = new Queue<Construction>();
    public Construction currentConstruction;

    public float energyProduction;
    public float energyConsumption;
    public float energyDemand;
    public CircularBuffer<float> recentEnergyProduction = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentEnergyConsumption = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentEnergyDemand = new CircularBuffer<float>(12);
    public int energyLevel = 1;
    public float energyProductionMultiplier = 1;

    private BodyInfo bodyInfo;

    private AlertData energyAlert;
    private TooltipData energyTooltip = new TooltipData("No energy!", "", "One or more buldings require more energy on ");
    [SerializeField]
    private Sprite energyAlertIcon;

    public class Construction
    {
        public Building building;
        public DateTime startDate;
        public BuildingButton button;
        public bool notEnoughEnergy;

        public Construction(Building building, DateTime startDate)
        {
            this.building = building;
            this.startDate = startDate;
        }
    }
    void Start()
    {
        recentPops.Add(population);

        DateManager.instance.OnDateUpdate += UpdateColony;

        bodyInfo = GetComponent<BodyInfo>();

        energyTooltip.sub = bodyInfo.name;
        energyTooltip.content += bodyInfo.name;
        energyAlert = new AlertData(AlertType.Colony, energyTooltip, true, () => PlanetModalManager.Instance.Spawn(bodyInfo, 2), energyAlertIcon);

    }
    int lastMonth = 0;
    public void UpdateColony()
    {
        if(lastMonth != DateManager.currentDate.Month)
        {
            // These things should be calculated monthly

            lastMonth = DateManager.currentDate.Month;


            population += (int)(population * (populationGrowthRate + Random.Range(-.5f, .5f)));
            recentPops.Add(population);

            recentStability.Add(stability);


            // Scaling by pops, stability, 
            gdp += gdp * gdpChangeRate * Random.Range(-.5f, .5f);
            recentGDP.Add(gdp);

            CalculateConsuption();
            recentEnergyProduction.Add(energyProduction);
            recentEnergyConsumption.Add(energyConsumption);
            recentEnergyDemand.Add(energyDemand);

            OnColonyUpdate?.Invoke();
        }

        UpdateConstruciton();
    }

    public void AddBuildingToQueue(Building building)
    {
        constructionQueue.Enqueue(new Construction(building, DateManager.currentDate));
        if (constructionQueue.Count == 1)
        {
            currentConstruction = constructionQueue.Peek();
        }

        CalculateConsuption();
        OnColonyUpdate?.Invoke();
    }

    void UpdateConstruciton()
    {
        if (constructionQueue.Count > 0)
        {
            if (currentConstruction.notEnoughEnergy)
            {
                return;
            }

            float constructionTime = currentConstruction.building.constructionTime / ColoniesManager.instance.constructionSpeed;
            float progress = (float)(DateManager.currentDate - currentConstruction.startDate).TotalDays;

            if(currentConstruction.button != null && currentConstruction.button.gameObject.activeSelf) currentConstruction.button.UpdateFill(progress / constructionTime);

            if(progress >= constructionTime)
            {
                constructionQueue.Dequeue();

                buildings.Add(new PlacedBuilding(currentConstruction.building));
                avaliableSlots -= 1;

                CalculateConsuption();

                if (constructionQueue.Count > 0)
                {
                    constructionQueue.Peek().startDate = DateManager.currentDate;

                    currentConstruction = constructionQueue.Peek();
                }

                OnColonyUpdate?.Invoke();
            }
        }
    }

    public void CalculateConsuption()
    {
        energyConsumption = 0;
        energyProduction = 0;
        energyDemand = 0;

        foreach (PlacedBuilding building in buildings)
        {
            if (!building.active) continue;
            energyProduction += building.building.energyProduction;
            energyDemand += building.building.energyConsumption;
        }

        energyProduction += ColoniesManager.instance.energyProductionByLevel[energyLevel];
        energyProduction *= energyProductionMultiplier; // High production multiplier should fuck smth up

        energyConsumption = energyDemand;

        int idx = buildings.Count;
        while(energyConsumption > energyProduction) // Try to disable as many buildings to save energy
        {
            idx--;
            if (idx < 0) break;

            buildings[idx].notEnoughEnergy = true;
            energyConsumption -= buildings[idx].building.energyConsumption;
        }

        if (constructionQueue.Count > 0)
        {
            energyDemand += ColoniesManager.instance.construcitonEnergyCost;

            if(energyDemand > energyProduction) constructionQueue.Peek().notEnoughEnergy = true;
            else energyConsumption += ColoniesManager.instance.construcitonEnergyCost;
        }

        if(energyProduction < energyDemand)
            AlertsManager.Instance.ShowAlert(energyAlert);
    }
}
