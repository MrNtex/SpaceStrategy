using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float panSpeed = 2000f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float minY = 20f;
    public float maxY = 120f;

    public float rotationSpeed = 20f;
    



    // Update is called once per frame
    void Update()
    {
        float shiftMultiplier = 1;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            shiftMultiplier = 2;
        }

        Vector3 input = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        if(input != Vector3.zero)
        {
            MoveCamera(input, shiftMultiplier);
        }

        Vector3 mouseScroll = Input.mouseScrollDelta;
        if (mouseScroll.y != 0)
        {
            MoveCamera(mouseScroll.y * 8 * transform.forward, 1, false);
        }
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse X"));
            transform.Rotate(Vector3.right * rotationSpeed * -Input.GetAxis("Mouse Y"));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

    }
    void MoveCamera(Vector3 dir, float shiftMultiplier = 1, bool breakFromParent = true)
    {
        transform.position += dir * panSpeed * Time.deltaTime * shiftMultiplier;
        if(transform.parent != null && breakFromParent)
        {
            transform.SetParent(null);
            Focus.focusedObject = null;
        }
    }
}
