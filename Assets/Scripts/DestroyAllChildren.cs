using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllChildren : MonoBehaviour
{
    public static void DestroyAllChildrenOf(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
