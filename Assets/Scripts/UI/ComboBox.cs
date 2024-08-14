using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBox : UIElement
{
    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject separator;

    public void Show(ComboBoxItem[] items)
    {
        DestroyAllChildren.DestroyAllChildrenOf(tooltip.transform);

        tooltip.SetActive(true);

        Vector2 pos = Input.mousePosition / canvasRect.GetComponent<Canvas>().scaleFactor;
        
        tooltip.transform.position = pos;

        for (int i = 0; i < items.Length; i++)
        {
            Instantiate(buttonPrefab, tooltip.transform).GetComponent<ComboBoxItemPrefab>().Set(items[i].text, items[i].icon, items[i].action);
            if (i < items.Length - 1)
            {
                Instantiate(separator, tooltip.transform);
            }
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
