using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmendmentsManager : MonoBehaviour
{
    public static AmendmentsManager Instance { get; private set; }

    public Dictionary<int, Amendment> amendments = new Dictionary<int, Amendment>();

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public int progress;
    public bool active;

    public Dictionary<string, int> effects; // Tag, Value

    public Sprite background;

    public Amendment(string name, string description, float cost, int duration, bool available, Dictionary<string, int> effects, Sprite background)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.duration = duration;
        this.available = available;
        this.progress = 0;
        this.active = false;
        this.effects = effects;
        this.background = background;
    }
}