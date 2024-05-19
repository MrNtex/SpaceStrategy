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

    private void Update()
    {
        if(activeDecision == null)
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
        if(activeDecision == null)
        {
            Debug.LogWarning("Finished null decision");
            return;
        }

        foreach (KeyValuePair<string, int> effect in activeDecision.Value.effects)
        {
            Effects.instance.ApplyEffect(effect.Key, effect.Value);
        }

        foreach (int next in activeDecision.Value.next)
        {
            if (DecisionsJSON.Instance.decisions.ContainsKey(next))
            {
                Decision decision = DecisionsJSON.Instance.decisions[next];
                if(avalibleDecisions.Contains(decision))
                {
                    continue;
                }
                avalibleDecisions.Add(DecisionsJSON.Instance.decisions[next]);
            }
            else
            {
                Debug.LogWarning($"Decision {next} not found");
            }
            
        }

        activeDecision = null;

        SetButtons();
        activeDecisionButton.SetUp();
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
        duration = decision.cost;
        endDate = startDate.AddDays(duration);

        activeDecisionButton.SetUp(decision);

        NationalUnity.instance.GenerateSupportForADecision(decision);
    }
}
