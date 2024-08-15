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

    private LeftPanelButton[] group;
    [SerializeField]
    private GameObject groupPrefab, leftPanelEfectPrefab;
    public void Create(LeftPanelEffect effect)
    {
        icon.sprite = effect.icon;
        if(effect is LeftPanelButton)
        {
            LeftPanelButton buttonEffect = effect as LeftPanelButton;

            button.onClick.AddListener(() => buttonEffect.action());
        }
        else if(effect is LeftPanelGroup)
        {
            LeftPanelGroup groupEffect = effect as LeftPanelGroup;

            group = groupEffect.buttons;
        }
        else
        {
            Debug.LogError("Unknown effect type");
        }

        tooltip = MenusManager.Instance.mainTooltip;

        tooltipData = new TooltipData
        (
            effect.name,
            "",
            effect.description
        );

        groupPrefab.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(group != null && group.Length > 0)
        {
            DestroyAllChildren.DestroyAllChildrenOf(groupPrefab.transform);

            groupPrefab.SetActive(true);

            foreach (LeftPanelButton button in group)
            {
                GameObject buttonObj = Instantiate(leftPanelEfectPrefab, groupPrefab.transform);
                buttonObj.GetComponent<LeftPanelEffectUI>().Create(button);
            }
        }

        tooltip.Show(tooltipData, TooltipTarget.LeftPanelEffect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();

        if (groupPrefab.activeSelf)
        {
            groupPrefab.SetActive(false);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        tooltip.MoveTooltip(eventData.position);
    }
}
