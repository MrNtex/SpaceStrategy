using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmendmentsManager : MonoBehaviour
{
    public static AmendmentsManager Instance { get; private set; }

    public Dictionary<int, Amendment> amendments = new Dictionary<int, Amendment>();

    private List<Amendment> activeAmendments = new List<Amendment>();

    public Dictionary<Amendment, AmendmentButton> buttons = new Dictionary<Amendment, AmendmentButton>();

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
                AddAmendment(amendment.Value);
            }
        }
    }
    void AddAmendment(Amendment amendment)
    {
        AmendmentButton button = Instantiate(amendmentButtonPrefab, modal).GetComponent<AmendmentButton>();
        button.Create(amendment);
        buttons.Add(amendment, button);
    }
    private void HandleDateChanged()
    {
        List<Amendment> toRemove = new List<Amendment>();
        for (int i = 0; i < activeAmendments.Count; i++)
        {
            Amendment amendment = activeAmendments[i];
            amendment.progress = (float)(DateManager.currentDate - amendment.startDate).TotalDays;
            if(amendment.progress >= amendment.duration)
            {
                foreach (KeyValuePair<string, int> effect in amendment.effects)
                {
                    // Apply effects
                    Debug.Log($"Applying effect {effect.Key} with value {effect.Value}");
                }

                toRemove.Add(amendment);
                buttons[amendment].Finish();
                continue;
            }
            activeAmendments[i] = amendment;

            buttons[amendment].UpdateFiller();
        }
        foreach (Amendment i in toRemove)
        {
            activeAmendments.Remove(i);
        }
    }
    public void SelectAmendment(Amendment amendment)
    {
        if (activeAmendments.Contains(amendment))
        {
            return;
        }

        StartAmendment(amendment);

    }

    private void StartAmendment(Amendment amendment)
    {
        if (amendment.cost > 100) // Change when money system is implemented
        {
            Debug.LogWarning("Not enough money to start amendment");
            return;
        }
        amendment.startDate = DateManager.currentDate;
        amendment.progress = 0;
        activeAmendments.Add(amendment);
        buttons[amendment].Activate();
    }
}

[System.Serializable]
public struct Amendment
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