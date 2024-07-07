using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Focus : MonoBehaviour
{
    public static ObjectFocusHelper focusedObject;

    public float focusTime;
    private bool isFocusing = false;

    private float startTime;

    private GameObject oldPos;
    private Transform cameraFocus;

    [SerializeField]
    private AnimationCurve focusCurve;


    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public delegate void OnLeftClick();
    public OnLeftClick onLeftClick;

    private GameObject dummyFocus;

    private void Start()
    {
        oldPos = new GameObject("OldPos");

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if(dummyFocus == null)
        {
            dummyFocus = new GameObject("DummyFocus");
        }
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
        ObjectFocusHelper dest = null;

        if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current != null &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            onLeftClick?.Invoke(); // This helps clicking on the smaller planets

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != focusedObject && hit.transform.GetComponent<ObjectFocusHelper>())
                {

                    dest = hit.transform.GetComponent<ObjectFocusHelper>();
                    FocusOn(dest.Focus());
                    return;
                }
            }
            else
            {
                BodyInfoUI.instance.SetBody(null);
                FleetManager.instance.selectedFleet = null;
                focusedObject = null;
            }

        }

        if (Input.GetKeyDown(KeyCode.F) && focusedObject != null)
        {
            if (focusedObject.target == null || focusedObject.target.gameObject.GetComponent<ObjectFocusHelper>() == null)
            {
                FocusOn(initialPosition, initialRotation);
                return;
            }

            dest = focusedObject.target.gameObject.GetComponent<ObjectFocusHelper>();
            FocusOn(dest, true);
        }
    }

    public void FocusOn(ObjectFocusHelper obj, bool force = false)
    {
        BodyInfoUI.instance.SetBody(obj.objectInfo);
        

        if (obj != focusedObject && !force)
        {
            // Only move the camera if double clicked
            focusedObject = obj;
            return;
        }

        if (cameraFocus == obj.cameraPlacement)
        {
            // If camera is already focused on the object, show the body info (it could've disapeared)
            //return;
        }

        FleetManager.instance.selectedFleet = null;

        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus = obj.cameraPlacement;
        startTime = Time.time;
        isFocusing = true;
    }
    private void FocusOn(Vector3 pos, Quaternion rot)
    {
        focusedObject = null;

        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus = dummyFocus.transform;
        cameraFocus.position = pos;
        cameraFocus.rotation = rot;
        startTime = Time.time;
        isFocusing = true;

    }
}
