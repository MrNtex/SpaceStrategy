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
    public List<Building> buildings = new List<Building>();

    public Queue<Construction> constructionQueue = new Queue<Construction>();
    public Construction currentConstruction;

    public float energyProduction;
    public float energyConsumption;
    public CircularBuffer<float> recentEnergyProduction = new CircularBuffer<float>(12);
    public CircularBuffer<float> recentEnergyConsumption = new CircularBuffer<float>(12);
    public int energyLevel = 1;

    public class Construction
    {
        public Building building;
        public DateTime startDate;
        public BuildingButton button;

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
            float constructionTime = currentConstruction.building.constructionTime / ColoniesManager.instance.constructionSpeed;
            float progress = (float)(DateManager.currentDate - currentConstruction.startDate).TotalDays;

            if(currentConstruction.button != null && currentConstruction.button.gameObject.activeSelf) currentConstruction.button.UpdateFill(progress / constructionTime);

            if(progress >= constructionTime)
            {
                constructionQueue.Dequeue();

                buildings.Add(currentConstruction.building);
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

    void CalculateConsuption()
    {
        energyConsumption = 0;
        energyProduction = 0;
        if(constructionQueue.Count > 0)
        {
            energyConsumption = ColoniesManager.instance.construcitonEnergyCost;
        }

        foreach(Building building in buildings)
        {
            if(building.energy < 0)
                energyConsumption -= building.energy;
            else
                energyProduction += building.energy;
        }

        energyProduction += ColoniesManager.instance.energyProductionByLevel[energyLevel];
    }
}
