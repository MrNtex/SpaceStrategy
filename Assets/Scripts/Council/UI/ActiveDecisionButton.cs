using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ActiveDecisionButton : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image progressBar, background;

    [SerializeField]
    private TMP_Text decisionName;

    [SerializeField]
    private GameObject decisionModal;

    [SerializeField]
    private Tooltip tooltip;

    private const string sub = "SHIFT for more info";

    private string advancedContent;

    private bool tooltipActive;

    private void Start()
    {
        SetUp();
    }
    private void OnEnable()
    {
        DateManager.instance.OnDateUpdate += HandleDateChange;
    }
    private void OnDisable()
    {
        DateManager.instance.OnDateUpdate -= HandleDateChange;
    }
    void HandleDateChange()
    {
        if (DecisionsManger.instance.activeDecision == -1)
        {
            return;
        }
        float progress = (float)DecisionsManger.instance.progress/DecisionsManger.instance.duration;
        progressBar.fillAmount = progress;

        if(tooltipActive && tooltip.tooltip.activeSelf && tooltip.target == TooltipTarget.ActiveDecision)
        {
            SetTooltip();
        }
    }

    public void OnClick()
    {
        if(DecisionsManger.instance.activeDecision != -1)
        {
            return;
        }
        decisionModal.SetActive(true);
    }

    public void SetUp(Decision decision)
    {
        progressBar.fillAmount = 0;
        decisionName.text = decision.name;
        background.sprite = decision.background;

        advancedContent = "Effects:\n";
        foreach (var effect in decision.effects)
        {
            advancedContent += $"{effect.Key}: {effect.Value}\n";
        }
    }

    public void SetUp()
    {
        progressBar.fillAmount = 0;
        decisionName.text = "No active decision";
        background.sprite = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        tooltip.MoveTooltip(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();

        tooltipActive = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipActive = true;

        if (DecisionsManger.instance.activeDecision != -1)
        {
            SetTooltip();
        }
    }

    private void SetTooltip()
    {
        Decision decision = DecisionsManger.instance.decisions[DecisionsManger.instance.activeDecision];

        int daysRemaining = (int)(DecisionsManger.instance.duration - DecisionsManger.instance.progress);

        Dictionary<string, string> countriesLiking = decision.coutriesLiking.ToDictionary(kvp => kvp.Key, kvp => kvp.ToString());


        TooltipData tooltipData = new TooltipData(decision.name, sub, $"Days remaining: {daysRemaining}", decision.name, "", $"Progress: {DecisionsManger.instance.progress.ToString("N2")}/{DecisionsManger.instance.duration} \n{advancedContent}", new Dictionary<string, string>(), countriesLiking);
        tooltip.ShowTooltip(tooltipData, TooltipTarget.ActiveDecision);
    }
}
