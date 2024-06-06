using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlertObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public AlertType alertType;

    public TooltipData? tooltipData;
    [SerializeField]
    private Tooltip tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!tooltipData.HasValue)
        {
            return;
        }
        tooltip.ShowTooltip((TooltipData)tooltipData);
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
        AlertsManager.Instance.OnAlertClick(alertType);
        tooltip.HideTooltip();
    }
    public void UpdateTooltipData(TooltipData? tooltipData)
    {
        this.tooltipData = tooltipData;
    }
}
