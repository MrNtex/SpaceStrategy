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
            case "unlock_decision":
                AmendmentsManager.Instance.UnlockDecision(val);
                break;

            default:
                Debug.LogWarning($"Effect: {effect} not found");
                break;
        }
    }
}
