using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanelEffectUI : MonoBehaviour
{
    public Image icon;
    public Button button;

    public TooltipData tooltipData;
    public void Create(LeftPanelEffect effect)
    {
        icon.sprite = effect.icon;
        button.onClick.AddListener(() => effect.action());

        tooltipData = new TooltipData
        {
            header = effect.name,
            content = effect.description
        };
    }
}
