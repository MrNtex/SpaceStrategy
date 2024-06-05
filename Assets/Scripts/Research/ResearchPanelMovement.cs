using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchPanelMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // Start is called before the first frame update

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
        if(Input.mouseScrollDelta.y != 0)
        {
            rectTransform.localScale += new Vector3(Input.mouseScrollDelta.y * 0.1f, Input.mouseScrollDelta.y * 0.1f, 0);
        }
    }
}
