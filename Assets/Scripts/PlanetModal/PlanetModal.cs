using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanetModal : MonoBehaviour
{

    [SerializeField]
    private GameObject planetModal;

    [SerializeField]
    private TMP_Text planetName;

    [SerializeField]
    private TMP_Text hability, stability, population;

    public BodyInfo bodyInfo;
    ColonyStatus colonyStatus;
    public void Spawn(BodyInfo bodyInfo)
    {

        // Set the planet modal's text to the bodyInfo's name
        planetName.text = bodyInfo.name;

        colonyStatus = bodyInfo.colonyStatus;
        if (colonyStatus == null)
        {
            Debug.LogError("colonyStatus is null");
            return;
        }

        hability.text = colonyStatus.hability.ToString()+"%";
        stability.text = colonyStatus.stability.ToString()+"%";
        population.text = colonyStatus.population.ToString();

        this.bodyInfo = bodyInfo;

        ColoniesManager.instance.OnColonyUpdate += OnColonyUpdate;
    }

    public void DestroySelf()
    {
        MenusManager.activeModals.Remove(gameObject);
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        ColoniesManager.instance.OnColonyUpdate -= OnColonyUpdate;
    }
    void OnColonyUpdate()
    {
        // UpdateVariables

        hability.text = colonyStatus.hability.ToString() + "%";
        stability.text = colonyStatus.stability.ToString() + "%";
        population.text = colonyStatus.population.ToString();
    }
}
