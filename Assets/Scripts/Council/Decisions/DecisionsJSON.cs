using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionsJSON : MonoBehaviour
{
    public static DecisionsJSON Instance { get; private set; }



    public Sprite defaultBackground;

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
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Decisions");

        foreach (TextAsset file in jsonFiles)
        {
            Debug.Log($"Loaded file: {file.name}");

            // Parse each JSON file into a ResearchCategory object
            Decisions decisions = JsonUtility.FromJson<Decisions>(file.text);
            if (decisions.decisions == null)
            {
                Debug.LogError($"Failed to parse decisions from {file.name}");
                continue; // Skip this iteration if parsing failed
            }
            foreach (DecisionJSON decisionJSON in decisions.decisions)
            {
                Dictionary<string, int> effects = new Dictionary<string, int>();
                
                foreach (string effect in decisionJSON.effectsString)
                {
                    string[] effectSplit = effect.Split(':');
                    if (effectSplit.Length == 2)
                        effects.Add(effectSplit[0], int.Parse(effectSplit[1]));
                    else
                        effects.Add(effectSplit[0], 0);
                }

                Dictionary<string, int> countriesLiking = new Dictionary<string, int>();

                foreach (string countryLiking in decisionJSON.countriesLikingString)
                {
                    string[] countryLikingSplit = countryLiking.Split(':');
                    if (Countries.instance.countriesDict.ContainsKey(countryLikingSplit[0]))
                    {
                        countriesLiking.Add(countryLikingSplit[0], int.Parse(countryLikingSplit[1]));
                    }
                    else
                    {
                        Debug.LogWarning($"{file.name}, {decisionJSON.name}: {countryLikingSplit[0]} not found in countries dictionary");
                    }
                }

                Sprite background = JSONUtils.LoadSprite("Decisions/", decisionJSON.background);

                Decision decision = new Decision(decisionJSON.id, decisionJSON.name, decisionJSON.description, decisionJSON.cost, decisionJSON.next, effects, countriesLiking, background);
                DecisionsManger.instance.decisions.Add(decision.id, decision);
            }
            
            DecisionsManger.instance.SelectStarting();
        }
        
    }
    
}
[System.Serializable]
public struct Decisions
{
    public List<DecisionJSON> decisions;
}
[System.Serializable]
public struct DecisionJSON
{
    // JSON can't serialize dictionaries, so we have to use lists instead
    public int id;

    public string name;
    public string description;

    public int cost;

    public List<int> next;

    public List<string> effectsString;
    public List<string> countriesLikingString;

    public string background;
}

[System.Serializable]
public struct Decision
{
    public int id;

    public string name;
    public string description;

    public int cost;

    public List<int> next;

    public Dictionary<string, int> effects; // Tag, Value
    public Dictionary<string, int> coutriesLiking; // Tag, Value

    public Sprite background;
    public Decision(int id, string name, string description, int cost, List<int> next, Dictionary<string, int> effects, Dictionary<string, int> coutriesLiking, Sprite background)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.next = next;
        this.effects = effects;
        this.coutriesLiking = coutriesLiking;
        this.background = background;
    }
}