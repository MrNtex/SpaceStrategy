using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BillboardRightClick : MonoBehaviour, IPointerClickHandler
{
    private Billboard billboard;
    void Start()
    {
        billboard = transform.parent.parent.GetComponent<Billboard>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            billboard.RightClick();
        }
    }
}

