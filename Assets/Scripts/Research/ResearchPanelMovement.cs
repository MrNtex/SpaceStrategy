using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchPanelMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    ResearchTreeGenerator researchTreeGenerator;
    // Start is called before the first frame update
    void Start()
    {
        researchTreeGenerator = transform.parent.GetComponent<ResearchTreeGenerator>();
    }

    private RectTransform rectTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            rectTransform = researchTreeGenerator.categoryPanels[researchTreeGenerator.currentCategory].GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            rectTransform.localPosition += (Vector3)eventData.delta;
        }
    }
}
