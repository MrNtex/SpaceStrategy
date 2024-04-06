using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public string planetName;


    private Orbiting orbiting;


    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Transform target;

    private GameObject cameraPlacement;
    private void Awake()
    {
        if (planetName == "")
        {
            planetName = gameObject.name;
        }
        orbiting = GetComponent<Orbiting>();
        if (orbiting != null)
        {
            target = orbiting.target;

        }

        
    }
    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Planet " + planetName + " has no target");
        }
        else if(cameraPlacement == null)
        {
            cameraPlacement = new GameObject("CameraPlacement");
            cameraPlacement.transform.SetParent(transform);
            cameraPlacement.transform.localPosition = offset;
        }
    }

    public FocusData Focus()
    {
        FocusData focusData = new FocusData();
        if (target == null) return focusData;
        
        focusData.Position = cameraPlacement.transform.localPosition;

        focusData.Rotation = Quaternion.LookRotation((transform.position - cameraPlacement.transform.position).normalized);

        focusData.Parent = transform;

        return focusData;
    }

}
