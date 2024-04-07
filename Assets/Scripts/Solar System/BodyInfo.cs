using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInfo : MonoBehaviour
{
    public string bodyName;

    private void Awake()
    {
        if(bodyName == "") bodyName = gameObject.name;
    }
}
