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
    void FixedUpdate()
    {
        if (capitan == transform)
        {
            return;
        }

        

        float angle = capitan.transform.rotation.eulerAngles.y;

        Vector3 offset = Quaternion.Euler(0, angle, 0) * myOffset;
        Vector3 dest = capitan.position + offset;

        fleet.CalculateMovment(dest, transform, ref velocity);

        return;
    }
}
