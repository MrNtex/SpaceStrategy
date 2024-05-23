using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private void Start()
    {
        SetUp();
    }
    void Update()
    {
        if (DecisionsManger.instance.activeDecision == -1)
        {
            return;
        }
        float progress = (float)DecisionsManger.instance.progress/DecisionsManger.instance.duration;
        progressBar.fillAmount = progress;
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
        advancedContent += "\n";
        foreach (var liking in decision.coutriesLiking)
        {
            advancedContent += $"{liking.Key}: {liking.Value}\n";
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DecisionsManger.instance.activeDecision != -1)
        {
            Decision decision = DecisionsManger.instance.decisions[DecisionsManger.instance.activeDecision];

            int daysRemaining = (int)(DecisionsManger.instance.duration - DecisionsManger.instance.progress); 
            
            TooltipData tooltipData = new TooltipData(decision.name, sub, $"Days remaining: {daysRemaining}", decision.name, "", $"Progress: {DecisionsManger.instance.progress}/{DecisionsManger.instance.duration} \n{advancedContent}");
            tooltip.ShowTooltip(tooltipData);
        }
    }
}
