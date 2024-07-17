using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentGraph
{
    Population,
    GDP,
    Stability,
}
public class PlanetModal : Modal, IPieCharDataTarget
{

    [SerializeField]
    private GameObject planetModal;

    [SerializeField]
    private TMP_Text planetName;


    public BodyInfo bodyInfo;
    public ColonyStatus colonyStatus;

    
    [SerializeField]
    private Image topBar;


    [SerializeField]
    private PlanetModalPage[] pages;

    private int currentPageIndex = 0;
    public void Spawn(BodyInfo bodyInfo)
    {
        MenusManager.activeModals.Add(gameObject);

        // Set the planet modal's text to the bodyInfo's name
        planetName.text = bodyInfo.name;

        topBar.sprite = bodyInfo.background;

        colonyStatus = bodyInfo.colonyStatus;
        if (colonyStatus == null)
        {
            Debug.LogError("colonyStatus is null");
            return;
        }

        this.bodyInfo = bodyInfo;

        LoadPage(0);

        OnColonyUpdate();

        colonyStatus.OnColonyUpdate += OnColonyUpdate;
    }
    
    public void LoadPage(int idx)
    {
        pages[currentPageIndex].gameObject.SetActive(false);
        pages[idx].gameObject.SetActive(true);
        pages[idx].Create(this);
        currentPageIndex = idx;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        colonyStatus.OnColonyUpdate -= OnColonyUpdate;
    }
    void OnColonyUpdate()
    {
        // UpdateVariables
        pages[currentPageIndex].OnColonyUpdate();
    }

    public TooltipData GetTooltipData(int slice)
    {
        throw new System.NotImplementedException();
    }
}
