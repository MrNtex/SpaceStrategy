using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBoxItemPrefab : MonoBehaviour
{
    public Button button;
    public TMPro.TMP_Text text;
    public Image image;

    public void Set(string text, Sprite image, Action action)
    {
        this.text.text = text;
        button.onClick.AddListener(() => action());

        if(image != null)
            this.image.sprite = image;
        else 
            this.image.gameObject.SetActive(false);
    }
}
