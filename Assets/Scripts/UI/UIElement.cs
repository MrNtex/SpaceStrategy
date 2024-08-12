using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UIElement : MonoBehaviour
{
    public GameObject tooltip;
    private RectTransform tooltipRect;
    [SerializeField]
    protected RectTransform dragger, canvasRect;

    [SerializeField]
    protected Vector2 offset = new Vector2(30, 30);

    private Vector2 savedPos;
    void Start()
    {
        tooltipRect = tooltip.GetComponent<RectTransform>();

        HideTooltip();

        MenusManager.Instance.OnChangedMenu += HideTooltip;
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
    }
    public virtual void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    
}
