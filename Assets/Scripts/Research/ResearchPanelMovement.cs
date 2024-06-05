using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchPanelMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    ResearchUI researchUI;
    // Start is called before the first frame update
    void Start()
    {
        researchUI = transform.parent.GetComponent<ResearchUI>();
    }

    private RectTransform rectTransform;
    public void ChangeCategory()
    {
        rectTransform = researchUI.categoryPanels[researchUI.currentCategory].GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            rectTransform = researchUI.categoryPanels[researchUI.currentCategory].GetComponent<RectTransform>();
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
