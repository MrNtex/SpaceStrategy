using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityToCursor : MonoBehaviour
{
    public enum State
    {
        Min,
        Mid,
        Max
    }

    [SerializeField]
    private Image[] spriteRenderers;

    [SerializeField]
    private float minDistance = 100f, maxDistance = 600f;

    [SerializeField]
    private AnimationCurve curve;

    private State state = State.Min;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(Input.mousePosition, transform.position);

        if(distance < minDistance)
        {
            if (state == State.Min) return;
            SetOpacity(minDistance);
            state = State.Min;
            return;
        }
        state = State.Mid;

        SetOpacity(distance);
    }

    void SetOpacity(float distance)
    {
        float step = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float a = curve.Evaluate(step);

        foreach(Image img in spriteRenderers)
        {
            Color color = img.color;
            color.a = a;
            img.color = color;
        }
    }
}
