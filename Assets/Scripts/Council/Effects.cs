using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects instance;
    public void Awake()
    {
        instance = this;
    }
    public void ApplyEffect(string effect, int val)
    {
        switch (effect)
        {
            case "debug":
                Debug.Log("Debug effect applied");
                break;

            default:
                Debug.LogWarning($"Effect: {effect} not found");
                break;
        }
    }
}
