using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ComboBox : UIElement
{
    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject separator;

    public void Show(ComboBoxItem[] items, bool breakOnCameraMovement = true)
    {
        if(breakOnCameraMovement)
        {
            CameraControler.onCameraMove += Hide;
        }
        DestroyAllChildren.DestroyAllChildrenOf(tooltip.transform);

        tooltip.SetActive(true);

        Vector2 pos = Input.mousePosition / canvasRect.GetComponent<Canvas>().scaleFactor;
        
        tooltip.transform.position = pos;

        for (int i = 0; i < items.Length; i++)
        {
            Instantiate(buttonPrefab, tooltip.transform).GetComponent<ComboBoxItemPrefab>().Set(items[i].text, items[i].icon, items[i].action, this, true);
            if (i < items.Length - 1)
            {
                Instantiate(separator, tooltip.transform);
            }
        }
    }

    public void Hide()
    {
        CameraControler.onCameraMove -= Hide;

        tooltip.SetActive(false);
    }

    void Update()
    {
        if (tooltip.activeSelf && Input.GetMouseButton(0) && !RectTransformUtility.RectangleContainsScreenPoint(
          tooltipRect,
          Input.mousePosition))
        {
            Hide();
        }
    }
}
public struct ComboBoxItem
{
    public string text;
    public Sprite icon;
    public System.Action action;

    public ComboBoxItem(string text, Sprite icon, System.Action action)
    {
        this.text = text;
        this.icon = icon;
        this.action = action;
    }

    public ComboBoxItem(string text, System.Action action)
    {
        this.text = text;
        this.icon = null;
        this.action = action;
    }
}
