using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoniesManager : MonoBehaviour
{
    public static ColoniesManager instance;

    public List<ColonyStatus> colonies = new List<ColonyStatus>();

    public float constructionSpeed = 1;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple ColoniesManagers in scene");
            Destroy(gameObject);
        }
    }
}
