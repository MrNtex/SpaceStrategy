using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour
{
    public Transform objectToFollow; // The 3D object the UI will follow
    public Vector3 offset; // Offset from the object's position
    public Canvas canvas; // Reference to the Canvas

    void Update()
    {
        if (objectToFollow != null)
        {
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, objectToFollow.position + offset);

            // Check the canvas render mode
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                transform.position = screenPosition;
            }
            else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 localPoint);
                transform.localPosition = localPoint;
            }
        }
    }
}
