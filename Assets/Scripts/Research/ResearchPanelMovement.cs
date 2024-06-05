using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchPanelMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    const float minScale = .5f, maxScale = 3f;

    private RectTransform rectTransform;
    public void ChangeCategory()
    {
        rectTransform = ResearchUI.categoryPanels[ResearchUI.currentCategory].GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            rectTransform = ResearchUI.categoryPanels[ResearchUI.currentCategory].GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            rectTransform.localPosition += (Vector3)eventData.delta;
        }
    }

    void Update()
    {
        Vector2 mouseScroll = Input.mouseScrollDelta;
        if(mouseScroll.y != 0 && (rectTransform.localScale.x > minScale || mouseScroll.y > 0) && (rectTransform.localScale.x < maxScale || mouseScroll.y < 0))
        {
            rectTransform.localScale += new Vector3(mouseScroll.y * 0.1f, mouseScroll.y * 0.1f, 0);
        }

        if(mouseScroll.x != 0)
        {
            rectTransform.localPosition += new Vector3(-mouseScroll.x * 10, 0, 0);
        }
    }
}
