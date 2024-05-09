using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * DateManager.timeScale);
    }
}
