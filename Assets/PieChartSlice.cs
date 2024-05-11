using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieChartSlice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int sliceIndex;
    [SerializeField]
    private float maxScale = 1.3f, maxOffset = 0.5f;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private Image image;

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * maxScale;

        //float angle = image.fillAmount * 180;
        //transform.localPosition = originalPosition + Quaternion.Euler(0, 0, angle) * transform.up * 10;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
    }
    
}
