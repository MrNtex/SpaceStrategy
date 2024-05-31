using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PlanetModalManager : MonoBehaviour
{
    /// <summary>
    /// MAIN CANVAS SCRIPT
    /// </summary>

    public static PlanetModalManager Instance { get; private set; }

    [SerializeField]
    private GameObject planetModal;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"There are multiple instances of PlanetModalManager ({Instance}, {this})");
            Destroy(this);
        }
    }

    public void Spawn(BodyInfo bodyInfo)
    {
        Instantiate(planetModal, transform).GetComponent<PlanetModal>().Spawn(bodyInfo);
    }
}
