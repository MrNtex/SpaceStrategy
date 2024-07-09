using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TooltipTarget
{
    Empty,
    Alert,
    Piechart,
    ActiveDecision
}
public class Tooltip : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text header, sub, content;

    public GameObject tooltip;
    private RectTransform tooltipRect;
    [SerializeField]
    protected RectTransform dragger, canvasRect;

    [SerializeField]
    protected Vector2 offset = new Vector2(-5, -5);

    protected TooltipData tooltipData;

    private Vector2 savedPos;

    public TooltipTarget target;


    [SerializeField]
    protected GameObject countryPanel;

    protected List<GameObject> countries = new List<GameObject>();
    void Start()
    {
        tooltipRect = tooltip.GetComponent<RectTransform>();

        HideTooltip();

        MenusManager.Instance.OnChangedMenu += HideTooltip;
    }
    public void ShowTooltip(TooltipData tooltipData, TooltipTarget target)
    {
        this.target = target;

        tooltip.SetActive(true);

        this.tooltipData = tooltipData;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ShowAdvanced();
        }
        else
        {
            ShowBasic();
        }
    }
    public void MoveTooltip(Vector2 pos, bool savePos = true)
    {
        if (savePos)
        {
            savedPos = pos;
        }
        // It ensures that the tooltip is always inside the canvas
        Vector2 desiredPosition = pos + offset;

        Vector2 tooltipSize = tooltipRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        float clampedX = Mathf.Clamp(desiredPosition.x, 0, canvasSize.x - tooltipSize.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, tooltipSize.y, canvasSize.y);

        dragger.anchoredPosition = new Vector2(clampedX, clampedY);
    }
    public virtual void HideTooltip()
    {
        tooltip.SetActive(false);
        target = TooltipTarget.Empty;
    }

    protected virtual void ShowAdvanced()
    {
        if(!tooltipData.hasAdvanced)
        {
            ShowBasic();
            return;
        }

        foreach (GameObject country in countries)
        {
            Destroy(country);
        }

        header.text = tooltipData.advancedHeader;
        sub.text = tooltipData.advancedSub;
        content.text = tooltipData.advancedContent;

        foreach (KeyValuePair<string, string> country in tooltipData.countriesAdvanced)
        {
            GameObject cp = Instantiate(countryPanel, tooltip.transform);
            CountrySupportOnTooltip countrySupportOnTooltip = cp.GetComponent<CountrySupportOnTooltip>();

            countrySupportOnTooltip.icon.sprite = Countries.instance.countriesDict[country.Key].icon;
            countrySupportOnTooltip.text.text = country.Value;

            countries.Add(cp);
        }

        StartCoroutine(nameof(AdjustAndMoveTooltip)); // Advanced tooltip is usually bigger than basic one
    }
    protected virtual void ShowBasic()
    {
        foreach (GameObject country in countries)
        {
            Destroy(country);
        }
        header.text = tooltipData.header;
        sub.text = tooltipData.sub;
        content.text = tooltipData.content;

        foreach (KeyValuePair<string, string> country in tooltipData.countriesBasic)
        {
            GameObject cp = Instantiate(countryPanel, tooltip.transform);
            CountrySupportOnTooltip countrySupportOnTooltip = cp.GetComponent<CountrySupportOnTooltip>();

            countrySupportOnTooltip.icon.sprite = Countries.instance.countriesDict[country.Key].icon;
            countrySupportOnTooltip.text.text = country.Value;

            countries.Add(cp);
        }

        StartCoroutine(nameof(AdjustAndMoveTooltip));
    }
    void Update()
    {
        if (!tooltip.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShowAdvanced();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ShowBasic();
        }
    }
    IEnumerator AdjustAndMoveTooltip()
    {
        // Wait for the next frame to get the correct size of the tooltip, because layout groups are updated after Update() method
        yield return new WaitForEndOfFrame();
        MoveTooltip(savedPos, false);
    }
}
public struct TooltipData
{
    public string header, sub, content;
    public bool hasAdvanced;
    public string advancedHeader, advancedSub, advancedContent;

    public Dictionary<string, string> countriesBasic, countriesAdvanced;
    

    public TooltipData(string header, string sub, string content)
    {
        this.header = header;
        this.sub = sub;
        this.content = content;
        this.hasAdvanced = false;
        this.countriesBasic = new Dictionary<string, string>();
        this.countriesAdvanced = new Dictionary<string, string>();
        this.advancedHeader = "";
        this.advancedSub = "";
        this.advancedContent = "";
    }
    public TooltipData(string header, string sub, string content, Dictionary<string, string> countriesBasic)
    {
        this.header = header;
        this.sub = sub;
        this.content = content;
        this.hasAdvanced = false;
        this.countriesBasic = countriesBasic;
        this.countriesAdvanced = new Dictionary<string, string>();
        this.advancedHeader = "";
        this.advancedSub = "";
        this.advancedContent = "";
    }
    public TooltipData(string header, string sub, string content, string advancedHeader, string advancedSub, string advancedContent)
    {
        this.header = header;
        this.sub = sub;
        this.content = content;
        this.hasAdvanced = true;
        this.countriesBasic = new Dictionary<string, string>();
        this.countriesAdvanced = new Dictionary<string, string>();
        this.advancedHeader = advancedHeader;
        this.advancedSub = advancedSub;
        this.advancedContent = advancedContent;
    }
    public TooltipData(string header, string sub, string content, string advancedHeader, string advancedSub, string advancedContent, Dictionary<string, string> countriesBasic, Dictionary<string, string> countriesAdvanced)
    {
        this.header = header;
        this.sub = sub;
        this.content = content;
        this.hasAdvanced = true;
        this.countriesBasic = countriesBasic;
        this.countriesAdvanced = countriesAdvanced;
        this.advancedHeader = advancedHeader;
        this.advancedSub = advancedSub;
        this.advancedContent = advancedContent;
    }
}
