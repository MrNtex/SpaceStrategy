using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraRightClick : MonoBehaviour
{
    public delegate void OnRightClick(GameObject dest);
    public OnRightClick onRightClick;

    private GameObject target;

    private void Start()
    {
        target = new GameObject("CameraRightClick's target");
        target.tag = "Point";
    }
    // Update is called once per frame
    void Update()
    {
        /// <sumary>
        /// Path for fleets is CameraRightClick -> FleetManager -> Fleet (There capitan's course is set) -> FlyPattern (There the rest of the ships move)
        /// </sumary>
        if (Input.GetMouseButtonDown(1)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                onRightClick?.Invoke(hit.transform.gameObject); // Send the hit point to the event
            }
            else
            {
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Plane at y = 0
                float enter = 0.0f;
                if (groundPlane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);

                    target.transform.position = hitPoint;
                    onRightClick?.Invoke(target); // Send the hit point to the event

                    // Optionally, check if the hit point is within a certain range or area
                }
            }
            
        }
    }
}
