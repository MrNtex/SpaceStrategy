using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There are multiple SpriteManagers in the scene. Destoryed one attached to " + gameObject.name);
            Destroy(gameObject);
        }
    }


    public Sprite moon;
}
