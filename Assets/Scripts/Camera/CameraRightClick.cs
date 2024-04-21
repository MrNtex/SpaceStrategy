using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraRightClick : MonoBehaviour
{
    public delegate void OnRightClick(Vector3 dest);
    public OnRightClick onRightClick;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Ray hit: " + hit.transform.name);
            }
            else
            {
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Plane at y = 0
                float enter = 0.0f;
                if (groundPlane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    

                    onRightClick?.Invoke(hitPoint); // Send the hit point to the event

                    // Optionally, check if the hit point is within a certain range or area
                }
            }
            
        }
    }
}
