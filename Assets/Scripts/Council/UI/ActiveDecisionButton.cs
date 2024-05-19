using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveDecisionButton : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private GameObject decisionModal;
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
}
