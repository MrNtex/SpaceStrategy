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

    private GameObject oldPos;
    private Transform focus;
    private PlanetInfo planetInfo;

    [SerializeField]
    private AnimationCurve focusCurve;

    private CameraControler cameraControler;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private void Start()
    {
        oldPos = new GameObject("OldPos");
        cameraControler = GetComponent<CameraControler>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    
    private void Update()
    {

        // <summary>
        // Focusing on the planet with a smooth transition
        // </summary>
        if (isFocusing) {    
            float t = (Time.time - startTime) / focusTime;
            if (t >= 1)
            {
                isFocusing = false;
                transform.rotation = focus.rotation;
                if(focus.parent != null)
                    transform.SetParent(focus.parent);
                transform.position = focus.position;
            }
            else
            {
                transform.position = Vector3.Lerp(focus.position, oldPos.transform.position, focusCurve.Evaluate(t));
                transform.rotation = Quaternion.Slerp(focus.rotation, oldPos.transform.rotation, focusCurve.Evaluate(t));
            }
        }



        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<PlanetInfo>())
                {
                    planet = hit.transform.GetComponent<PlanetInfo>();
                    FocusOn(planet);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(planet.target == null)
            {
                FocusOn(initialPosition, initialRotation);
                return;
            }

            planet = planet.target.gameObject.GetComponent<PlanetInfo>();
            FocusOn(planet);
        }

    }
    private void FocusOn(PlanetInfo planet)
    {
        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        focus = planet.Focus();
        startTime = Time.time;
        isFocusing = true;
    }
    private void FocusOn(Vector3 pos, Quaternion rot)
    {
        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        focus.position = pos;
        focus.rotation = rot;
        startTime = Time.time;
        isFocusing = true;
    }
}
