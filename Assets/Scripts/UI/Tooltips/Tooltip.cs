using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text header, sub;

    public GameObject tooltip;
    [SerializeField]
    protected RectTransform dragger;

    [SerializeField]
    protected Vector2 offset = new Vector2(-5, -5);

    public void ShowTooltip(string header, string sub, bool advanced)
    {
        this.header.text = header;
        this.sub.text = sub;
    }
    public void MoveTooltip(Vector2 pos)
    {
        dragger.anchoredPosition = pos + offset;
    }
    public virtual void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
