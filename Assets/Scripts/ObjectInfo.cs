using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectInfo : MonoBehaviour
{
    public string objectName;

    protected ObjectFocusHelper objectFocusHelper;
    private CameraFocus cameraFocus;
    public BodyStatus bodyStatus;

    [SerializeField]
    private bool useCustomColor = false;
    [SerializeField]
    private Color color;

    public Orbiting orbiting;


    public Sprite icon;

    
    private void Awake()
    {
        if(objectName == "") objectName = gameObject.name;

        objectFocusHelper = GetComponent<ObjectFocusHelper>();
        cameraFocus = Camera.main.GetComponent<CameraFocus>();
        
    }
    
    private void Start()
    {
        if (useCustomColor)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        bodyStatus = GetComponent<BodyStatus>();
        orbiting = GetComponent<Orbiting>();
    }
    public virtual void ButtonClicked()
    {
        FleetManager.instance.selectedFleet = null; // Override this in the Fleet class

        cameraFocus.FocusOn(objectFocusHelper);
    }

    public virtual string GetDescrition()
    {
        return "This should be overridden in the subclass";
    }
    public virtual void SetStatus(ref TMP_Text text)
    {
        text.text = "";
        text.color = Color.white;
        return;
    }
    public virtual Sprite GetIcon()
    {
        return SpriteManager.instance.moon;
    }
    public virtual Color GetColor()
    {
        return Color.white;
    }
}
