using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiting : MonoBehaviour
{
    public Transform target;

    public const float speed = 1000.0f;
    private float actualSpeed;

    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float multiplier = 1.0f;

    public float elapsedSeconds = 0;

    [SerializeField]
    private DateManager dateManager;

    // Start is called before the first frame update
    void Start()
    {
        actualSpeed = speed/Vector3.Distance(transform.position, target.position);

        // Circle around the target
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 100;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                float angle = i * Mathf.PI * 2 / lineRenderer.positionCount;
                Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Vector3.Distance(transform.position, target.position);
                lineRenderer.SetPosition(i, pos);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, actualSpeed * Time.deltaTime * multiplier);

        if(dateManager != null)
        {
            // Update elapsed time
            elapsedSeconds += Time.deltaTime;

            // Assuming 360 degrees rotation equals one year (365 days)
            // Calculate elapsed days based on rotation speed and multiplier
            float elapsedDays = elapsedSeconds / (360f / (actualSpeed * multiplier)) * 365f;

            dateManager.UpdateDate((int)elapsedDays);
        }
        
    }
}
