using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlyPattern : MonoBehaviour
{
    public GameObject target;

    public GameObject targetObject; // Object has priority over vector

    [SerializeField]
    Transform capitan;

    public Vector3 myOffset;
    

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float speed = 5.0f;
    void Update()
    {
        if (capitan == transform)
        {
            this.enabled = false;
        }
        Vector3 dest = capitan.position + myOffset;
        transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocity, smoothTime, speed * DateManager.timeScale, Time.deltaTime);

    }
}
