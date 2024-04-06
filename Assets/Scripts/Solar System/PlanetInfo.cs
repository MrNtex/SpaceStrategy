using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public string planetName;

    private void Awake()
    {
        if (planetName == "")
        {
            planetName = gameObject.name;
        }
    }
}
