using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlanetModalPage : MonoBehaviour
{
    public abstract void OnColonyUpdate();
    public abstract void Create(PlanetModal planetModal);
}
