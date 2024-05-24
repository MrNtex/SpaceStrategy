using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionsManger : MonoBehaviour
{
    public static DecisionsManger instance;

    public Dictionary<int, Decision> decisions = new Dictionary<int, Decision>();

    public List<int> avalibleDecisions = new List<int>(); // Use indexes to avoid null references
    public int activeDecision; // -1 if no active decision

    public double progress = 0;
    public DateTime startDate;
    public DateTime endDate = new DateTime(2224, 6, 7);
    public float duration = 0;

    [SerializeField]
    private GameObject[] decisionButtonPrefab;

    [SerializeField]
    private ActiveDecisionButton activeDecisionButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DateManager.instance.OnDateUpdate += HandleDateChanged;
    }
    public void SelectStarting()
    {
        for (int i = 1; i < 4; i++)
        {
            // First 3 decisions are always available
            avalibleDecisions.Add(i);
        }
        SetButtons();
    }

    private void HandleDateChanged()
    {
        if(activeDecision == -1)
        {
            return;
        }
        progress = DateManager.currentDate.Subtract(startDate).TotalDays;
        
        if(progress >= duration)
        {
            FinishedDecision();
        }
    }

    void FinishedDecision()
    {
        if (activeDecision == -1)
        {
            Debug.LogWarning("Finished null decision");
            return;
        }

        foreach (KeyValuePair<string, int> effect in decisions[activeDecision].effects)
        {
            Effects.instance.ApplyEffect(effect.Key, effect.Value);
        }

        UnlockNextDecisions();

        ChangeRelations();

        PopupsManager.Instance.CreatePopup(decisions[activeDecision], true);

        activeDecision = -1;


        SetButtons();
        activeDecisionButton.SetUp();

        AlertsManager.Instance.ShowAlert(AlertType.Decision);
        
    }
    private void ChangeRelations()
    {
        foreach(KeyValuePair<string, int> country in decisions[activeDecision].coutriesLiking)
        {
            if (Countries.instance.countriesDict.ContainsKey(country.Key))
            {
                Country c = Countries.instance.countriesDict[country.Key];
                c.support += country.Value;
                c.support = Mathf.Clamp(c.support, 0, 200);
                Countries.instance.countriesDict[country.Key] = c;
            }
            else
            {
                Debug.LogWarning($"Country {country.Key} not found");
            }
        }
    }
    private void UnlockNextDecisions()
    {

        foreach (int next in decisions[activeDecision].next)
        {
            if (decisions.ContainsKey(next))
            {
                if (avalibleDecisions.Contains(next))
                {
                    continue;
                }
                avalibleDecisions.Add(next);
            }
            else
            {
                Debug.LogWarning($"Decision {next} not found");
            }

        }
    }

    public void SetButtons()
    {
        if(avalibleDecisions.Count == 0)
        {
            Debug.LogWarning("No avalible decisions");
            return;
        }
        
        if (avalibleDecisions.Count <= 3)
        {
            int i = 0;
            for (; i < avalibleDecisions.Count; i++)
            {
                decisionButtonPrefab[i].SetActive(true);
                decisionButtonPrefab[i].GetComponent<DecisionButton>().SetDecision(avalibleDecisions[i]);
            }
            for (; i < decisionButtonPrefab.Length; i++)
            {
                decisionButtonPrefab[i].SetActive(false);
            }
            return;
        }

        // Select 3 random decisions
        List<int> selectedDecisions = new List<int>();
        while(selectedDecisions.Count < 3)
        {
            int decisionIdx = avalibleDecisions[UnityEngine.Random.Range(0, avalibleDecisions.Count)];
            if (!selectedDecisions.Contains(decisionIdx))
            {
                selectedDecisions.Add(decisionIdx);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            decisionButtonPrefab[i].SetActive(true);
            decisionButtonPrefab[i].GetComponent<DecisionButton>().SetDecision(selectedDecisions[i]);
        }
    }

    public void SelectDecision(int decisionIdx)
    {
        
        activeDecision = decisionIdx;

        Decision decision = decisions[decisionIdx];
        startDate = DateManager.currentDate;
        duration = decision.cost;
        endDate = startDate.AddDays(duration);

        activeDecisionButton.SetUp(decision);

        NationalUnity.instance.GenerateSupportForADecision(decision);

        avalibleDecisions.Remove(decisionIdx);

        AlertsManager.Instance.HideAlert(AlertType.Decision);
    }
}
