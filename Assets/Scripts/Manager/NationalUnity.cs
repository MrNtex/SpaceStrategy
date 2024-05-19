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
    void Awake()
    {
        instance = this;
    }

    public void GenerateSupportForADecision(Decision decision)
    {
        Dictionary<Country, float> countriesLikingDecision = new Dictionary<Country, float>();
        

        foreach (KeyValuePair<string, int> country in decision.coutriesLiking)
        {
            Country c = Countries.instance.countriesDict[country.Key];
            float support = 0;
            
            if(country.Value < 0)
            {
                // Country dislikes the decision
                support += Mathf.Clamp(c.support + country.Value,-200, 0); // If country supports you, but dislikes the decision, it will remain neutral
                countriesLikingDecision.Add(c, support);
                continue;
            }
            support += country.Value;
            countriesLikingDecision.Add(c, support);
        }

        float[] supp = new float[3]; // 0: support, 1: neutral, 2: against
        foreach (KeyValuePair<Country, float> country in countriesLikingDecision)
        {
            if (country.Value > 5)
            {
                supp[0] += country.Key.diplomaticWeight;
            }
            else if(country.Value > -5)
            {
                supp[1] += country.Key.diplomaticWeight;
            }
            else
            {
                supp[2] += country.Key.diplomaticWeight;
            }
        }
        
        supp[0] *= Mathf.Clamp(Mathf.Log(nationalUnity, 50), 0.3f, 2); // Log with base 50, so that the support is extreamly low when national unity is close to 0, and multiplied when close to 100


        pieChart.values = new List<float> { supp[0], supp[1], supp[2] };
        pieChart.SetValues();
    }
}
