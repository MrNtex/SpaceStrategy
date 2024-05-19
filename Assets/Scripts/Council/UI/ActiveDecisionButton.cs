using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveDecisionButton : MonoBehaviour
{
    [SerializeField]
    private Image progressBar, background;

    [SerializeField]
    private TMP_Text decisionName;

    [SerializeField]
    private GameObject decisionModal;

    private void Start()
    {
        SetUp();
    }
    void Update()
    {
        if (DecisionsManger.instance.activeDecision == null)
        {
            return;
        }
        float progress = (float)DecisionsManger.instance.progress/DecisionsManger.instance.duration;
        progressBar.fillAmount = progress;
    }

    public void OnClick()
    {
        if(DecisionsManger.instance.activeDecision != null)
        {
            return;
        }
        decisionModal.SetActive(true);
    }

    public void SetUp(Decision decision)
    {
        progressBar.fillAmount = 0;
        decisionName.text = decision.name;
        background.sprite = decision.background;
    }

    public void SetUp()
    {
        progressBar.fillAmount = 0;
        decisionName.text = "No active decision";
        background.sprite = null;
    }
}
