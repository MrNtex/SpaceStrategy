using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CelestialBillboardRightClick : MonoBehaviour, IPointerClickHandler
{
    private CelestailBilboard celestailBilboard;
    void Start()
    {
        celestailBilboard = transform.parent.GetComponent<CelestailBilboard>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            celestailBilboard.RightClick();
        }
    }
}

