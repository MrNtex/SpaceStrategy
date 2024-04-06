using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField]
    private PlanetInfo planet;

    public float focusTime;
    private bool isFocusing = false;

    private float startTime;
    FocusData focusData;
    private FocusData focusDataStart;

    [SerializeField]
    private AnimationCurve focusCurve;
    private void Update()
    {
        if (isFocusing) {    
            float t = (Time.time - startTime) / focusTime;
            if (t >= 1)
            {
                isFocusing = false;
                transform.rotation = focusData.Rotation;
                transform.SetParent(focusData.Parent);
                transform.localPosition = focusData.Position;
            }
            else
            {
                transform.position = Vector3.Lerp(focusData.Position, focusData.Parent.position + focusDataStart.Position, focusCurve.Evaluate(t));
                transform.rotation = Quaternion.Slerp(focusData.Rotation, focusDataStart.Rotation, focusCurve.Evaluate(t));
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            focusData = planet.Focus();

            startTime = Time.time;
            isFocusing = true;
        }

    }
}

public struct FocusData
{
    public Vector3 Position;
    public Quaternion Rotation;

    public Transform Parent;

    public FocusData(Vector3 position, Quaternion rotation, Transform parent)
    {
        Position = position;
        Rotation = rotation;
        Parent = parent;
    }
}
