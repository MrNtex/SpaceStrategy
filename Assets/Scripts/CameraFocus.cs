using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public ObjectFocusHelper planet;


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

    private GameObject dummyFocus;

    [SerializeField]
    private BodyInfoUI bodyInfoUI;

    private void Start()
    {
        oldPos = new GameObject("OldPos");
        cameraControler = GetComponent<CameraControler>();

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

        if (Input.GetMouseButtonDown(0))
        {
            onLeftClick?.Invoke(); // This helps clicking on the smaller planets


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != planet && hit.transform.GetComponent<ObjectFocusHelper>())
                {
                    dest = hit.transform.GetComponent<ObjectFocusHelper>();
                    FocusOn(dest);
                }
            }
            else
            {
                bodyInfoUI.SetBody(null);
            }

        }

        if (Input.GetKeyDown(KeyCode.F) && planet != null)
        {
            if (planet.target == null)
            {
                FocusOn(initialPosition, initialRotation);
                return;
            }

            dest = planet.target.gameObject.GetComponent<ObjectFocusHelper>();
            FocusOn(dest);
        }
    }

    public void FocusOn(ObjectFocusHelper obj)
    {
        if(this.planet == obj)
        {
            bodyInfoUI.SetBody(obj.objectInfo);
            return;
        }
        this.planet = obj.Focus();
        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus = obj.cameraPlacement;
        startTime = Time.time;
        isFocusing = true;

        bodyInfoUI.SetBody(obj.objectInfo);
    }
    private void FocusOn(Vector3 pos, Quaternion rot)
    {
        planet = null;

        oldPos.transform.position = transform.position;
        oldPos.transform.rotation = transform.rotation;

        cameraFocus = dummyFocus.transform;
        cameraFocus.position = pos;
        cameraFocus.rotation = rot;
        startTime = Time.time;
        isFocusing = true;

        bodyInfoUI.SetBody(null);
    }
}
