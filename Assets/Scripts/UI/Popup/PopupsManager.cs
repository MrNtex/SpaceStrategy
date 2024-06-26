using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PopupsManager : MonoBehaviour
{
    public static PopupsManager Instance;

    [SerializeField]
    private GameObject popupDefault;
    void Awake()
    {
        Instance = this;
    }

    public void CreatePopup<T>(T arg, bool pause = false) {
        
        DateManager.instance.Pause(true);
        if (arg is Decision decision)
        {
            Popup decisionPopup = Instantiate(popupDefault, transform).GetComponent<Popup>();
            decisionPopup.Create("Finished decision", decision.name, 2, decision.background);
            MenusManager.activeModals.Add(decisionPopup.gameObject);
        }
        else
        {
            Debug.LogError("Argument is not of type Decision.");
        }
    }
}
