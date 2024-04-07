using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;

    public float rotationSpeed = 20f;
    // Start is called before the first frame update

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * panSpeed * Time.deltaTime;
            MoveCamera(transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveCamera(-transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveCamera(transform.right);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveCamera(-transform.right);
        }
        Vector3 mouseScroll = Input.mouseScrollDelta;
        if (mouseScroll.y != 0)
        {
            MoveCamera(mouseScroll.y * 8 * transform.forward, false);
        }
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse X"));
            transform.Rotate(Vector3.right * rotationSpeed * -Input.GetAxis("Mouse Y"));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

    }
    void MoveCamera(Vector3 dir, bool breakFromParent = true)
    {
        transform.position += dir * panSpeed * Time.deltaTime;
        if(transform.parent != null && breakFromParent)
        {
            transform.SetParent(null);
        }
    }

    public void ResetToDefault()
    {
        transform.SetParent(null);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
