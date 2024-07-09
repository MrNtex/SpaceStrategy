using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NationalUnity : MonoBehaviour
{
    [SerializeField]
    private PieChart pieChart;
    [SerializeField]
    private TMP_Text nationalUnityText;

    public static NationalUnity instance;

    [SerializeField]
    private float defaultNationalUnity = 84;

    public float nationalUnity {
        get
        {
            return _nationalUnity;
        }
        set
        {
            _nationalUnity = Mathf.Clamp(value, 0, 100);
            nationalUnityText.text = _nationalUnity.ToString();
        }
    }
    private float _nationalUnity = 84;
    // Start is called before the first frame update

    public List<List<CountryOpinion>> countryOpinions = new List<List<CountryOpinion>>();
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        nationalUnity = defaultNationalUnity;
    }
    public void GenerateSupportForADecision(Decision decision)
    {
        countryOpinions.Clear();
        countryOpinions.Add(new List<CountryOpinion>());
        countryOpinions.Add(new List<CountryOpinion>());
        countryOpinions.Add(new List<CountryOpinion>());


        Dictionary<Country, CountryOpinion> countriesLikingDecision = new Dictionary<Country, CountryOpinion>();
        

        foreach (KeyValuePair<string, int> country in decision.coutriesLiking)
        {
            Country c = Countries.instance.countriesDict[country.Key];
            float support = 0;
            
            if(country.Value < 0)
            {
                // Country dislikes the decision
                float likingSupport = Mathf.Clamp(country.Value + c.support, -200, 0); // If country dislikes the decision, but national unity is high, it will remain neutral
                support += likingSupport; // If country supports you, but dislikes the decision, it will remain neutral
                countriesLikingDecision.Add(c, new CountryOpinion(c, support, country.Value, c.support, c.diplomaticWeight));
                continue;
            }
            support += country.Value;
            countriesLikingDecision.Add(c, new CountryOpinion(c, support, country.Value, -1, c.diplomaticWeight));
        }

        float[] supp = new float[3]; // 0: support, 1: neutral, 2: against
        foreach (KeyValuePair<Country, CountryOpinion> country in countriesLikingDecision)
        {
            if (country.Value.value > 5)
            {
                supp[0] += country.Key.diplomaticWeight;
                countryOpinions[0].Add(country.Value);
            }
            else if(country.Value.value > -5)
            {
                supp[1] += country.Key.diplomaticWeight;
                countryOpinions[1].Add(country.Value);
            }
            else
            {
                supp[2] += country.Key.diplomaticWeight;
                countryOpinions[2].Add(country.Value);
            }
        }
        
        supp[0] *= Mathf.Clamp(Mathf.Log(nationalUnity, 50), 0.3f, 2); // Log with base 50, so that the support is extreamly low when national unity is close to 0, and multiplied when close to 100


        pieChart.values = new List<float> { supp[0], supp[1], supp[2] };
        pieChart.SetValues();
    }

    public TooltipData GetTooltipData(int slice)
    {
        List<CountryOpinion> countryOpinionsForDecision = countryOpinions[slice];
        TooltipData tooltipData = new TooltipData();
        
        switch (slice)
        {
            case 0:
                tooltipData.header = "Support";
                tooltipData.advancedContent = $"The value is being multiplied by {Mathf.Clamp(Mathf.Log(nationalUnity, 50), 0.3f, 2)} due to national unity.";
                break;
            case 1:
                tooltipData.header = "Abstained";
                break;
            case 2:
                tooltipData.header = "Opposition";
                break;
        }

        tooltipData.sub = "SHIFT for advanced information";

        tooltipData.countriesBasic = new Dictionary<string, string>();
        tooltipData.countriesAdvanced = new Dictionary<string, string>();

        foreach (CountryOpinion opinion in countryOpinionsForDecision)
        {
            tooltipData.countriesBasic.Add(opinion.country.tag, opinion.diplomaticWeight.ToString());
            if (opinion.overallSupport == -1)
            {
                tooltipData.countriesAdvanced.Add(opinion.country.tag, $"{opinion.value} = {opinion.decisionSupport}. Country has a diplomatic weight of {opinion.diplomaticWeight}");
                continue;
            }
            tooltipData.countriesAdvanced.Add(opinion.country.tag, $"{opinion.value} = {opinion.decisionSupport} + {opinion.overallSupport}. Country has a diplomatic weight of {opinion.diplomaticWeight}");
        }
        return tooltipData;
    }
}

public struct CountryOpinion
{
    public Country country;
    public float value;

    // Hidden by default
    public float decisionSupport;
    public float overallSupport;
    public float diplomaticWeight;

    public CountryOpinion(Country country, float value, float decisionSupport, float overallSupport, float diplomaticWeight)
    {
        this.country = country;
        this.value = value;
        this.decisionSupport = decisionSupport;
        this.overallSupport = overallSupport;
        this.diplomaticWeight = diplomaticWeight;
    }
}
