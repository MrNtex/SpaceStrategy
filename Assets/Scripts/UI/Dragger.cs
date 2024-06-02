using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]
    private Transform target;

    private Vector2 offset;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = (Vector2)target.localPosition - eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        target.localPosition = eventData.position + offset;
    }
}
