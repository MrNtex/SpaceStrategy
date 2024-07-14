using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public virtual void OnDestroy()
    {
        MenusManager.activeModals.Remove(gameObject);
    }
}
