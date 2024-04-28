using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererScaler : MonoBehaviour
{
    // DO NOT USE NOW, CAUSES LAG

    Transform mainCamera;
    LineRenderer lineRenderer;

    [SerializeField]
    private float startDistance, endDistance;

    public float width;
    private void Start()
    {
        mainCamera = Camera.main.transform;
        lineRenderer = GetComponent<LineRenderer>();

        width = lineRenderer.startWidth;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(mainCamera.position, transform.position);
        if(distance < startDistance)
        {
            if(distance < endDistance)
            {
                SetWidth(0);
                return;
            }

            SetWidth(Mathf.Clamp(width * (distance - endDistance) / endDistance, 0, width));
        }
    }
    void SetWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}
