using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmendmentsJSON : MonoBehaviour
{
    void Start()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Amendments");

        foreach (TextAsset file in jsonFiles)
        {
            // Parse each JSON file into a ResearchCategory object
            AmendmentsBufforJSON buffor = JsonUtility.FromJson<AmendmentsBufforJSON>(file.text);
            if (buffor.amendments == null)
            {
                Debug.LogError($"Failed to parse decisions from {file.name}");
                continue; // Skip this iteration if parsing failed
            }
            foreach (AmendmentJSON amendmentJSON in buffor.amendments)
            {
                Sprite background = JSONUtils.LoadSprite("Amendments/", amendmentJSON.background);

                Amendment amendment = new Amendment(amendmentJSON.name, amendmentJSON.description, amendmentJSON.cost, amendmentJSON.duration, amendmentJSON.availableByDefault, amendmentJSON.effectsString.ToArray(), background);
                AmendmentsManager.Instance.amendments.Add(amendmentJSON.id, amendment);
            }
        }
        AmendmentsManager.Instance.UpdateAmendments();
    }
}
[System.Serializable]
public struct AmendmentsBufforJSON
{
    public List<AmendmentJSON> amendments;
}
[System.Serializable]
public struct AmendmentJSON
{
    public int id;

    public string name;
    public string description;

    public float cost;
    public int duration;

    public bool availableByDefault;

    public List<string> effectsString;

    public string background;
}