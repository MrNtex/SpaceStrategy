using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupportTooltip : MonoBehaviour
{
    [SerializeField]
    private TMP_Text header, sub;
    public GameObject tooltip;

    [SerializeField]
    private GameObject countryPanel;

    List<GameObject> countries = new List<GameObject>();
    [SerializeField]
    private RectTransform dragger;

    [SerializeField]
    private Vector2 offset = new Vector2(-5, -5);

    private bool shiftPressed = false;
    private int currentSlice = 0;
    public void ShowTooltip(int slice, bool advanced)
    {
        currentSlice = slice;
        Clear();

        if(NationalUnity.instance.countryOpinions.Count <= slice)
        {
            // This happens when the tooltip is called before the country opinions are set, or when piechar is empty
            return;
        }
        List<CountryOpinion> countryOpinions = NationalUnity.instance.countryOpinions[slice];
        tooltip.SetActive(true);

        if(!advanced && shiftPressed)
        {
            advanced = true;
        }

        sub.text = "";
        if (!advanced)
        {
            sub.text = "SHIFT for advanced information";
        }

        switch (slice)
        {
            case 0:
                header.text = "Support";
                if(advanced)
                {
                    sub.text = $"The value is being multiplied by {Mathf.Clamp(Mathf.Log(NationalUnity.instance.nationalUnity, 50), 0.3f, 2)} due to national unity.";
                }
                break;
            case 1:
                header.text = "Abstained";
                break;
            case 2:
                header.text = "Opposition";
                break;
        }
        foreach(CountryOpinion opinion in countryOpinions)
        {
            GameObject cp = Instantiate(countryPanel, tooltip.transform);
            CountrySupportOnTooltip countrySupportOnTooltip = cp.GetComponent<CountrySupportOnTooltip>();
            
            countrySupportOnTooltip.icon.sprite = opinion.country.icon;
            countrySupportOnTooltip.text.text = GetSupportText(opinion, advanced);

            countries.Add(cp);
        }
    }
    public void MoveTooltip(Vector2 pos)
    {
        dragger.anchoredPosition = pos + offset;
    }
    public void HideTooltip()
    {
        Clear();
        tooltip.SetActive(false);
    }

    private void Clear()
    {
        foreach (GameObject country in countries)
        {
            Destroy(country);
        }
        countries.Clear();
    }

    string GetSupportText(CountryOpinion opinion, bool advanced)
    {
        if (!advanced)
        {
            return opinion.country.name + ": " + opinion.diplomaticWeight;
        }
        else
        {
            if (opinion.overallSupport == -1)
            {
                return $"{opinion.country.name}: {opinion.value} = {opinion.decisionSupport}. Country has a diplomatic weight of {opinion.diplomaticWeight}";
            }
            return $"{opinion.country.name}: {opinion.value} = {opinion.decisionSupport} + {opinion.overallSupport}. Country has a diplomatic weight of {opinion.diplomaticWeight}";
        }
    }

    private void Update()
    {
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        if(tooltip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ShowTooltip(currentSlice, true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ShowTooltip(currentSlice, false);
            }
        }
        
    }
}
