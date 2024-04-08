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


        const float JupiterScaleBasedSize = 13f; // Jupiter's scale as a base for size calculation
        const float DistanceScaleFactor = 10000f;
        const float MinScaleMultiplier = 0.016f;
        const float MaxScaleMultiplier = 0.08f;
        const float TargetLerpDistance = 2.636f; // Specific distance for Lerp

        // SCALE
        // Calculating size scalar based on the parent's scale to adjust for Jupiter's scale
        float sizeScalar = JupiterScaleBasedSize / transform.parent.localScale.x;

        // Clamping the scale multiplier between min and max values, then adjusting it based on the size scalar
        float scaleMultiplier = Mathf.Clamp(distance / DistanceScaleFactor, MinScaleMultiplier, MaxScaleMultiplier) * sizeScalar;

        transform.localScale = Vector3.one * scaleMultiplier;

        // POSITION
        // Adjusting position based on target's right side, distance from target, and the interpolation factor
        float t = 1 - scaleMultiplier / (MaxScaleMultiplier * sizeScalar);
        transform.position = target.position + rightOfTarget * Mathf.Lerp(distanceFromTarget, TargetLerpDistance, t);

        transform.position += new Vector3(0, YOffset, 0);

        transform.LookAt(transform.position - directionToCamera);
    }
    public void Underline(bool start)
    {
        if(start) text.fontStyle = FontStyles.Underline;
        else text.fontStyle = FontStyles.Normal;
    }
}
