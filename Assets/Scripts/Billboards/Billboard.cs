using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Billboard : MonoBehaviour
{
    protected float minDistance; // Overriden by the PlanetFocusHelper

    protected Transform target; // Your planet's transform
    const float distanceFromTarget = 75; // Distance from the target to place the text
    public float YOffset = 1f; // Height from the target to place the text
    public float maxFontSize = 0.1f; // Maximum font size
    public Camera mainCamera;
    [SerializeField]
    protected GameObject button;

    [SerializeField]
    protected TMP_Text text;

    protected Vector3 inverseParentScale;

    protected virtual void Start()
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


        Vector3 cumulativeScale = CalculateCumulativeParentScale(transform);
        // Inverse the parents scale to keep the text size consistent (it has to be cumulative because of moons)

        inverseParentScale = new Vector3(1 / cumulativeScale.x, 1 / cumulativeScale.y, 1 / cumulativeScale.z);
    }

    

    Vector3 CalculateCumulativeParentScale(Transform currentTransform)
    {
        Vector3 totalScale = Vector3.one; // Start with no scale

        while (currentTransform.parent != null) // Traverse up the hierarchy
        {
            // Multiply the current total scale by this parent's scale
            totalScale = Vector3.Scale(totalScale, currentTransform.parent.localScale);
            currentTransform = currentTransform.parent; // Move up the hierarchy
        }

        return totalScale;
    }
    protected virtual void FixedUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector3 directionToCamera = mainCamera.transform.position - target.position;

        float distance = directionToCamera.magnitude;

        if (distance > minDistance && minDistance != -1)
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


        
        const float DistanceScaleFactor = 600f;
        const float MinScaleMultiplier = 0.1f;
        const float MaxScaleMultiplier = 1;
        const float TargetLerpDistance = 2.636f; // Specific distance for Lerp

        // SCALE
        
        // Clamping the scale multiplier between min and max values, then adjusting it based on the size scalar

        float scaleMultiplier = Mathf.Clamp(distance / DistanceScaleFactor, MinScaleMultiplier, MaxScaleMultiplier);

        transform.localScale = inverseParentScale * scaleMultiplier;

        // POSITION
        // Adjusting position based on target's right side, distance from target, and the interpolation factor
        float t = 1 - scaleMultiplier / (MaxScaleMultiplier);
        transform.position = target.position + rightOfTarget * Mathf.Lerp(distanceFromTarget, TargetLerpDistance, t);

        transform.position += new Vector3(0, YOffset, 0);

        transform.LookAt(transform.position - directionToCamera);
    }
}
