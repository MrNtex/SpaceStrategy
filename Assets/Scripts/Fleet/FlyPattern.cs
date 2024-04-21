using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPattern : MonoBehaviour
{
    public Vector3 target;
    [SerializeField]
    Transform capitan;

    public Vector3 myOffset;
    

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float speed = 5.0f;
    void Update()
    {
        if(capitan == null || capitan == transform)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, speed, Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, capitan.position + myOffset, ref velocity, smoothTime, speed, Time.deltaTime);
        }
    }
}
