using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target; // Your planet's transform
    public float distanceFromTarget = 1f; // Distance from the target to place the text
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector3 directionToCamera = (mainCamera.transform.position - target.position).normalized;

        // Calculate a position to the right of the target by using the cross product, which gives a perpendicular vector
        Vector3 rightOfTarget = Vector3.Cross(directionToCamera, Vector3.up);


        transform.position = target.position + rightOfTarget * distanceFromTarget;
        transform.LookAt(transform.position - directionToCamera);

        Vector3 euler = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, euler.y, 0);
    }
}
