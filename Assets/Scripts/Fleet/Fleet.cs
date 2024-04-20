using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShipType
{
    Fighter,
    Destroyer,
    Cruiser,
    Battleship,
    Dreadnought
}
public class Fleet : MonoBehaviour
{
    public GameObject capitan;
    public GameObject[] ships;

    public string fleetName;

    public Ship[] composition;

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public struct Ship
{
    public ShipType type;
    public int count;
}
