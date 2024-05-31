using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private int openPanelID;

    [SerializeField]
    private Image background;
    public void Create(string title, string description, int openPanelID, Sprite background)
    {
        // Create popup with title, description and options
        titleText.text = title;
        descriptionText.text = description;
        this.openPanelID = openPanelID;
        this.background.sprite = background;
    }

    public void Close()
    {
        MenusManager.activeModals.Remove(gameObject);

        Destroy(gameObject);
        DateManager.instance.Resume();
        
    }

    public void OpenPanel()
    {
        // Open panel with ID
        MenusManager.Instance.ChangeMenu(openPanelID);
        Destroy(gameObject);
        //Close();
    }
}
