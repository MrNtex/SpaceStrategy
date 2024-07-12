using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoniesManager : MonoBehaviour
{
    public static ColoniesManager instance;

    public List<ColonyStatus> colonies = new List<ColonyStatus>();

    public delegate void ColonyUpdate();
    public event ColonyUpdate OnColonyUpdate;

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

    // Start is called before the first frame update
    void Start()
    {
        DateManager.instance.OnMonthUpdate += UpdateColonies;
    }

    // Update is called once per frame
    void UpdateColonies()
    {
        foreach (ColonyStatus colony in colonies)
        {
            colony.UpdateColony();
        }

        OnColonyUpdate?.Invoke();
    }
}
