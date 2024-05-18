using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DecisionButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text decisionText;
    [SerializeField]
    private SpriteRenderer background;

    public Decision decision;
    void SetDecision(Decision decision)
    {
        this.decision = decision;
        // Set the decision text
        decisionText.text = decision.name;

        // Set the decision color
        background.sprite = decision.background;
    }
}
