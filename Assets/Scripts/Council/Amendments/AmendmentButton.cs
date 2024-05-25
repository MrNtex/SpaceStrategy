using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmendmentButton : MonoBehaviour
{

    public Amendment amendment;

    [SerializeField]
    private TMP_Text titleText, subText, costText;

    [SerializeField]
    private Image background, filler;

    bool finished = false;
    public void Create(Amendment amendment)
    {
        this.amendment = amendment;

        titleText.text = amendment.name;
        subText.text = amendment.description;
        costText.text = amendment.cost.ToString();

        background.sprite = amendment.background;
        filler.fillAmount = amendment.progress / amendment.duration;
    }
    public void Activate()
    {
        filler.fillAmount = 0;
        finished = false;
    }
    public void UpdateFiller()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        filler.fillAmount = amendment.progress / amendment.duration;
    }

    public void OnClick()
    {
        if (finished)
        {
            finished = false;
            return;
        }
        AmendmentsManager.Instance.SelectAmendment(amendment);
    }
    public void Finish()
    {
        filler.fillAmount = 0;
        finished = true;
    }
}
