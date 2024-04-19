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


    [SerializeField]
    private DateManager dateManager;

    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = target.position - transform.localPosition;
        
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
    void FixedUpdate()
    {
        float speed = actualSpeed * multiplier * DateManager.timeScale;
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);

        if(dateManager != null)
        {
            float elapsedDays = Vector3.Angle(target.position - transform.localPosition, lastPosition) * 1.0145f;

            if (elapsedDays >= 1)
            {
                dateManager.UpdateDate(elapsedDays);
                lastPosition = target.position - transform.localPosition;
            }

        }
        
    }
}
