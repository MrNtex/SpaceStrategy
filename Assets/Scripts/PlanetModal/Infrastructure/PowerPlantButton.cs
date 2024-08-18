using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerPlantButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    Tooltip tooltip;

    [SerializeField]
    private PlanetModal planetModal;

    private ColonyStatus colonyStatus;

    private Dictionary<int, string> powerPlantTitles = new Dictionary<int, string>
    {
        {1, "Nuclear Power Plant"},
        {2, "Fusion Power Plant"},
        {3, "Antimatter Power Plant"},
        {4, "Zero-point Power Plant"},
    };

    void Start()
    {
        tooltip = MenusManager.Instance.mainCanvas.GetComponent<Tooltip>();
        colonyStatus = planetModal.colonyStatus;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipData data = new TooltipData(powerPlantTitles[colonyStatus.powerPlantLevel], "", "");
        tooltip.Show(data, TooltipTarget.Colony);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        tooltip.MoveTooltip(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
