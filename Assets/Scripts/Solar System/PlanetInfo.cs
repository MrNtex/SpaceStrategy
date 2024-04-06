using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public string planetName;


    private Orbiting orbiting;
    private Transform target;

    [SerializeField]
    private Vector3 focusOffset = new Vector3(0, 1, 0);
    // Every time we focus on a planet, we will move the camera to the position of the planet plus the focusOffset and minus the normalized vector from the target to the planet times 20
    private float focusDistance = 50f;

    [SerializeField]
    private float planetWeight = .8f;
    // The weight of the planet in the solar system. The higher the weight, the more the camera will focus on this planet


    private Camera mainCamera;
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

        mainCamera = Camera.main;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (planetName != "Jupiter") return;
            Focus();
        }
        
    }
    public void Focus()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 pos = transform.position + focusOffset - dir * focusDistance;
        mainCamera.transform.position = pos;

        Vector3 targetLookAt = Vector3.Lerp((target.position - mainCamera.transform.position).normalized, (transform.position - mainCamera.transform.position).normalized, planetWeight);

        mainCamera.transform.rotation = Quaternion.LookRotation(targetLookAt, Vector3.up);

        mainCamera.transform.parent = transform;
    }
}
