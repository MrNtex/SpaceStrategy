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

    private Fleet fleet;

    private void Start()
    {
        fleet = transform.parent.GetComponent<Fleet>();

        if(fleet == null)
        {
            Debug.LogError("FlyPattern: Fleet not found");
        }
    }
    void Update()
    {
        if (capitan == transform)
        {
            return;
        }
        Vector3 dest = capitan.position + myOffset;
        transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocity, smoothTime, fleet.speed, Time.deltaTime * DateManager.timeScale);

    }
}
