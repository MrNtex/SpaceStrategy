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


    public void Spawn(BodyInfo bodyInfo)
    {

        // Set the planet modal's text to the bodyInfo's name
        planetName.text = bodyInfo.name;

        BodyStatus bodyStatus = bodyInfo.bodyStatus;

        hability.text = bodyStatus.hability.ToString()+"%";
        stability.text = bodyStatus.stability.ToString()+"%";
        population.text = bodyStatus.population.ToString();
    }
}
