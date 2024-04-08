using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField]
    private PlanetFocusHelper planet;

    public GameObject focusedObject;

    public float focusTime;
    private bool isFocusing = false;

    private float startTime;

    private GameObject oldPos;
    private Transform cameraFocus;

    [SerializeField]
    private AnimationCurve focusCurve;

    private CameraControler cameraControler;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public delegate void OnLeftClick();
    public OnLeftClick onLeftClick;

    private GameObject focusObject;

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
        if (isFocusing)
        {
            float t = (Time.time - startTime) / focusTime;
            if (t >= 1)
            {
                isFocusing = false;
                transform.rotation = cameraFocus.rotation;
                if (cameraFocus.parent != null)
                {
                    transform.SetParent(cameraFocus.parent);
                    focusedObject = cameraFocus.parent.gameObject;
                }
                    
                transform.position = cameraFocus.position;
            }
            else
            {
                transform.position = Vector3.Lerp(cameraFocus.position, oldPos.transform.position, focusCurve.Evaluate(t));
                transform.rotation = Quaternion.Slerp(cameraFocus.rotation, oldPos.transform.rotation, focusCurve.Evaluate(t));
            }
        }

        FocusChange();

    }

    private void FocusChange()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            onLeftClick?.Invoke(); // This helps clicking on the smaller planets


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != planet && hit.transform.GetComponent<PlanetFocusHelper>())
                {
                    planet = hit.transform.GetComponent<PlanetFocusHelper>();
                    FocusOn(planet);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (planet.target == null)
            {
                FocusOn(initialPosition, initialRotation);
                return;
            }

            planet = planet.target.gameObject.GetComponent<PlanetFocusHelper>();
            FocusOn(planet);
        }
    }

    public void FocusOn(PlanetFocusHelper planet)
    {
        if(planet.gameObject == focusedObject)
        {
            return;
        }
        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus = planet.Focus();
        startTime = Time.time;
        isFocusing = true;
    }
    private void FocusOn(Vector3 pos, Quaternion rot)
    {
        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus.position = pos;
        cameraFocus.rotation = rot;
        startTime = Time.time;
        isFocusing = true;
    }
}
