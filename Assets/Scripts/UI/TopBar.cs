using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public bool topBarLocked = false;

    [SerializeField]
    private AnimationCurve curve;

    public GameObject arrow;
    public GameObject topBar;

    [SerializeField]
    private Image arrowImage;

    private const float arrowY = 75;
    private const float topBarY = 65;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow.activeInHierarchy)
        {
            float mouseY = Input.mousePosition.y;
            float screenPercent = (mouseY / Screen.height) * 100f;
            
            float diff = screenPercent - arrowY;
            if(diff > 0)
            {
                float val = diff / (100 - arrowY);
                arrowImage.color = new Color(1, 1, 1, curve.Evaluate(val*2));
            }
        }else if(topBar.activeInHierarchy && !topBarLocked)
        {
            float mouseY = Input.mousePosition.y;
            float screenPercent = (mouseY / Screen.height) * 100f;

            float diff = screenPercent - topBarY;
            if(diff < 0)
            {
                topBar.SetActive(false);
                arrow.SetActive(true);
            }
        }
    }

    public void Hover()
    {
        arrow.SetActive(false);
        topBar.SetActive(true);
    }
}
