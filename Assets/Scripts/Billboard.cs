using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float minDistance = 3000; // Overriden by the PlanetFocusHelper

    private Transform target; // Your planet's transform
    public float distanceFromTarget = 1f; // Distance from the target to place the text
    public float YOffset = 1f; // Height from the target to place the text
    public float maxFontSize = 540f; // Maximum font size
    public Camera mainCamera;

    private TMP_Text text;

    void Start()
    {
        target = transform.parent;
        if (target == null)
        {
            Debug.LogError("Billboard script must be a child of the object it is supposed to follow");
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        text = GetComponent<TMP_Text>();

        BodyInfo bodyInfo = target.GetComponent<BodyInfo>();
        if (bodyInfo != null)
        {
            text.text = bodyInfo.bodyName;
        }
    }
    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector3 directionToCamera = mainCamera.transform.position - target.position;

        float distance = directionToCamera.magnitude;

        if (distance > minDistance)
        {
            text.enabled = false;
            return;
        }
        else
        {
            text.enabled = true;
        }

        directionToCamera.Normalize();

        // Calculate a position to the right of the target by using the cross product, which gives a perpendicular vector
        Vector3 rightOfTarget = Vector3.Cross(directionToCamera, transform.up);


        transform.position = target.position + rightOfTarget * distanceFromTarget;
        transform.position = new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
        transform.LookAt(transform.position - directionToCamera);

        
        float fontSize = Mathf.Clamp(distance / 4, 1, maxFontSize);
        text.fontSize = fontSize;
    }
}
