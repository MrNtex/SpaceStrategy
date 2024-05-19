using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text decisionText, descriptionText;
    [SerializeField]
    private Image background;

    public int decisionIdx;
    public void SetDecision(int decisionIdx)
    {
        this.decisionIdx = decisionIdx;
        // Set the decision text
        decisionText.text = DecisionsManger.instance.decisions[decisionIdx].name;
        descriptionText.text = DecisionsManger.instance.decisions[decisionIdx].description;

        // Set the decision color
        background.sprite = DecisionsManger.instance.decisions[decisionIdx].background;
    }
    public void OnClick()
    {
        DecisionsManger.instance.SelectDecision(decisionIdx);
    }
}
