using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmendmentsManager : MonoBehaviour
{
    public static AmendmentsManager Instance { get; private set; }

    public Dictionary<int, Amendment> amendments = new Dictionary<int, Amendment>();

    [SerializeField]
    private List<int> activeAmendments = new List<int>();

    public Dictionary<int, AmendmentButton> buttons = new Dictionary<int, AmendmentButton>();

    [SerializeField]
    private GameObject amendmentButtonPrefab;
    [SerializeField]
    private Transform modal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
    public void UpdateAmendments()
    {
        foreach (KeyValuePair<int, Amendment> amendment in amendments)
        {
            if (amendment.Value.available)
            {
                AddAmendment(amendment.Key);
            }
        }
    }
    public void UnlockDecision(int amendmentID)
    {
        if (!amendments.ContainsKey(amendmentID))
        {
            Debug.LogWarning($"Amendment with ID {amendmentID} not found");
            return;
        }
        amendments[amendmentID].available = true;
        AddAmendment(amendmentID);
    }
    void AddAmendment(int amendmentID)
    {
        AmendmentButton button = Instantiate(amendmentButtonPrefab, modal).GetComponent<AmendmentButton>();
        buttons.Add(amendmentID, button);

        button.Create(amendments[amendmentID], amendmentID);
        
    }
    public void HandleDateChanged()
    {
        List<int> toRemove = new List<int>();
        for (int i = 0; i < activeAmendments.Count; i++)
        {
            amendments[activeAmendments[i]].progress = (float)(DateManager.currentDate - amendments[activeAmendments[i]].startDate).TotalDays;
            if(amendments[activeAmendments[i]].progress >= amendments[activeAmendments[i]].duration)
            {
                Finish(toRemove, i);
                continue;
            }
            amendments[activeAmendments[i]] = amendments[activeAmendments[i]];

            buttons[activeAmendments[i]].UpdateFiller(amendments[activeAmendments[i]].progress / amendments[activeAmendments[i]].duration);
        }
        foreach (int i in toRemove)
        {
            activeAmendments.Remove(i);
        }
    }

    private void Finish(List<int> toRemove, int i)
    {
        foreach (KeyValuePair<string, int> effect in amendments[activeAmendments[i]].effects)
        {
            // Apply effects
            Effects.instance.ApplyEffect(effect.Key, effect.Value);
        }

        toRemove.Add(activeAmendments[i]);
        buttons[activeAmendments[i]].Finish();
    }

    public void SelectAmendment(int amendment)
    {
        if (activeAmendments.Contains(amendment))
        {
            return;
        }

        StartAmendment(amendment);

    }

    private void StartAmendment(int amendmentID)
    {
        Amendment amendment = amendments[amendmentID];

        if (amendment.cost > 100) // Change when money system is implemented
        {
            Debug.LogWarning("Not enough money to start amendment");
            return;
        }
        
        amendment.startDate = DateManager.currentDate;
        amendment.progress = 0;
        activeAmendments.Add(amendmentID);
        buttons[amendmentID].Activate();
    }
}

[System.Serializable]
public class Amendment
{
    public string name;
    public string description;
    
    public float cost;
    public int duration;
    
    public bool available;

    public DateTime startDate;
    public float progress;

    public Dictionary<string, int> effects; // Tag, Value

    public Sprite background;

    public Amendment(string name, string description, float cost, int duration, bool available, Dictionary<string, int> effects, Sprite background)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.duration = duration;
        this.available = available;
        this.startDate = DateManager.currentDate;
        this.progress = 0;
        this.effects = effects;
        this.background = background;
    }
}