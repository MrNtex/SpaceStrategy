using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionsManger : MonoBehaviour
{
    public static DecisionsManger instance;

    public List<Decision> avalibleDecisions = new List<Decision>();
    public Decision? activeDecision;

    public double progress = 0;
    public DateTime startDate;
    public DateTime endDate = new DateTime(2224, 6, 7);

    [SerializeField]
    private GameObject[] decisionButtonPrefab;

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

    private void Update()
    {
        if(activeDecision == null)
        {
            return;
        }
        progress = endDate.Subtract(DateManager.currentDate).TotalDays;
        Debug.Log(progress);
        if(progress <= 0)
        {
            FinishedDecision();
        }
    }

    void FinishedDecision()
    {
        if(activeDecision == null)
        {
            Debug.LogWarning("Finished null decision");
            return;
        }

        foreach (KeyValuePair<string, int> effect in activeDecision.Value.effects)
        {
            Effects.instance.ApplyEffect(effect.Key, effect.Value);
        }

        foreach(int next in activeDecision.Value.next)
        {
            avalibleDecisions.Add(DecisionsJSON.Instance.decisions[next]);
        }

        activeDecision = null;

        SetButtons();
    }

    void SetButtons()
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
            }
            for (; i < decisionButtonPrefab.Length; i++)
            {
                decisionButtonPrefab[i].SetActive(false);
            }
            return;
        }

        // Select 3 random decisions
        List<Decision> selectedDecisions = new List<Decision>();
        while(selectedDecisions.Count < 3)
        {
            Decision decision = avalibleDecisions[UnityEngine.Random.Range(0, avalibleDecisions.Count)];
            if (!selectedDecisions.Contains(decision))
            {
                selectedDecisions.Add(decision);
            }
        }
    }

    public void SelectDecision(Decision decision)
    {
        avalibleDecisions.Remove(decision);

        activeDecision = decision;
        startDate = DateManager.currentDate;
    }
}
