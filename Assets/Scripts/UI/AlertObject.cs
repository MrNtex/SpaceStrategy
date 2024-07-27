using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlertObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    public AlertData alertData;

    public TooltipData? tooltipData;
    [SerializeField]
    private Tooltip tooltip;

    [SerializeField]
    private Image icon, border;

    [SerializeField]
    private Color severeRim, normalRim;

    public void SetUp(AlertData alertData, Tooltip tooltip)
    {
        icon.sprite = alertData.icon;
        UpdateTooltipData(alertData.tooltipData);

        this.alertData = alertData;

        border.color = alertData.severe ? severeRim : normalRim;

        this.tooltip = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!tooltipData.HasValue)
        {
            return;
        }
        tooltip.Show((TooltipData)tooltipData, TooltipTarget.Alert);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        tooltip.MoveTooltip(eventData.position);
    }
    public void OnClick()
    {
        alertData.onClick.Invoke();
        tooltip.HideTooltip();
    }
    public void UpdateTooltipData(TooltipData? tooltipData)
    {
        this.tooltipData = tooltipData;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            AlertsManager.Instance.HideAlert(alertData);
        }
    }
}
