using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftPanelEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Image icon;
    public Button button;

    public TooltipData tooltipData;
    private Tooltip tooltip;
    public void Create(LeftPanelEffect effect)
    {
        icon.sprite = effect.icon;
        if(effect is LeftPanelButton)
        {
            LeftPanelButton buttonEffect = effect as LeftPanelButton;

            button.onClick.AddListener(() => buttonEffect.action());
        }

        tooltip = MenusManager.Instance.mainTooltip;

        tooltipData = new TooltipData
        (
            effect.name,
            "",
            effect.description
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(tooltipData, TooltipTarget.LeftPanelEffect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        tooltip.MoveTooltip(eventData.position);
    }
}
