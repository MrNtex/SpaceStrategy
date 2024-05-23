using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    void Start()
    {
        tooltipRect = tooltip.GetComponent<RectTransform>();

        HideTooltip();
    }
    public void ShowTooltip(TooltipData tooltipData)
    {
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
        Debug.Log($"Tooltip position: {dragger.anchoredPosition}");
    }
    public virtual void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    protected virtual void ShowAdvanced()
    {
        if(!tooltipData.hasAdvanced)
        {
            ShowBasic();
            return;
        }
        header.text = tooltipData.advancedHeader;
        sub.text = tooltipData.advancedSub;
        content.text = tooltipData.advancedContent;

        StartCoroutine(nameof(AdjustAndMoveTooltip)); // Advanced tooltip is usually bigger than basic one
    }
    protected virtual void ShowBasic()
    {
        header.text = tooltipData.header;
        sub.text = tooltipData.sub;
        content.text = tooltipData.content;

        StartCoroutine(nameof(AdjustAndMoveTooltip));
    }
    void Update()
    {
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
    
    public TooltipData(string header, string sub, string content)
    {
        this.header = header;
        this.sub = sub;
        this.content = content;
        this.hasAdvanced = false;
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
        this.advancedHeader = advancedHeader;
        this.advancedSub = advancedSub;
        this.advancedContent = advancedContent;
    }
}
