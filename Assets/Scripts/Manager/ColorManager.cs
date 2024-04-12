using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There are multiple SpriteManagers in the scene. Destoryed one attached to " + gameObject.name);
            Destroy(gameObject);
        }
    }


    [Header("Planet Statuses")]
    public Color colonized;
    public Color colonizable;
    public Color canBeTerraformed;
    public Color specialized;
    public Color canBeSpecialized;
    public Color inhabitable;
}
