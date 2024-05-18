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

    public Decision decision;
    void SetDecision(Decision decision)
    {
        this.decision = decision;
        // Set the decision text
        decisionText.text = decision.name;
        descriptionText.text = decision.description;

        // Set the decision color
        background.sprite = decision.background;
    }
    public void OnClick()
    {
        DecisionsManger.instance.SelectDecision(decision);
    }
}
