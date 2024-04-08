using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float minDistance = 3000; // Overriden by the PlanetFocusHelper

    private Transform target; // Your planet's transform
    const float distanceFromTarget = 75; // Distance from the target to place the text
    public float YOffset = 1f; // Height from the target to place the text
    public float maxFontSize = 0.1f; // Maximum font size
    public Camera mainCamera;
    [SerializeField]
    private GameObject button;

    [SerializeField]
    private TMP_Text text;

    void Start()
    {
        target = transform.parent;
        if (target == null)
        {
            Debug.LogError("Billboard script must be a child of the object it is supposed to follow");
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        

        BodyInfo bodyInfo = target.GetComponent<BodyInfo>();
        if (bodyInfo != null)
        {
            text.text = bodyInfo.bodyName;
        }
    }
    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector3 directionToCamera = mainCamera.transform.position - target.position;

        float distance = directionToCamera.magnitude;

        if (distance > minDistance)
        {
            button.SetActive(false);
            return;
        }
        else
        {
            button.SetActive(true);
        }

        directionToCamera.Normalize();

        // Calculate a position to the right of the target by using the cross product, which gives a perpendicular vector
        Vector3 rightOfTarget = Vector3.Cross(directionToCamera, transform.up);




        // Legacy code for changing the font size based on distance
        //float fontSize = Mathf.Clamp(distance / 4, 1, maxFontSize);
        //text.fontSize = fontSize;
        float sizeScalar = 13 / transform.parent.localScale.x; // 13 because it was based on the jupiter's scale
        float scaleMlt = Mathf.Clamp(distance / 8000, 0.016f, 0.08f) * sizeScalar;
        transform.localScale = Vector3.one * scaleMlt;
        float t = 1-scaleMlt / (0.08f* sizeScalar);
        transform.position = target.position + rightOfTarget * Mathf.Lerp(distanceFromTarget, 2.636f, t);
        transform.position = new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
        transform.LookAt(transform.position - directionToCamera);
    }
    public void Underline(bool start)
    {
        if(start) text.fontStyle = FontStyles.Underline;
        else text.fontStyle = FontStyles.Normal;
    }
}
