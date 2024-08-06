using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ObjectInfo : MonoBehaviour
{
    public string objectName;

    protected ObjectFocusHelper objectFocusHelper;
    protected Focus cameraFocus;

    public Sprite icon;

    public List<LeftPanelEffect> effects = new List<LeftPanelEffect>();

    
    private void Awake()
    {
        if(objectName == "") objectName = gameObject.name;

        objectFocusHelper = GetComponent<ObjectFocusHelper>();
        cameraFocus = CameraControler.mainCamera.GetComponent<Focus>();
        
    }
    public virtual void ButtonClicked()
    {
        cameraFocus.FocusOn(objectFocusHelper);
    }

    public virtual string GetDescription()
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
    public virtual void SpecialButtonClicked()
    {
        return;
    }
}
[System.Serializable]
public class LeftPanelEffect
{
    public string name;
    public string description;

    public Sprite icon;

    public Action action;

    public LeftPanelEffect[] additional;

    public LeftPanelEffect(string n, string d, Sprite i, Action a = null)
    {
        name = n;
        description = d;
        icon = i;
        action = a;
    }
    public LeftPanelEffect(string n, string d, Sprite i, LeftPanelEffect[] a, Action ac = null)
    {
        name = n;
        description = d;
        icon = i;
        action = ac;
        additional = a;
    }
}
