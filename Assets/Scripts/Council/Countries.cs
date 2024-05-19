using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countries : MonoBehaviour
{
    [SerializeField]
    private List<Country> countries;

    public static Countries instance;

    public Dictionary<string, Country> countriesDict = new Dictionary<string, Country>();
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        foreach (Country country in countries)
        {
            countriesDict.Add(country.tag, country);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public struct Country
{
    public string tag;
    public string name;

    public Sprite icon;

    public int diplomaticWeight;

    public float support;
}
