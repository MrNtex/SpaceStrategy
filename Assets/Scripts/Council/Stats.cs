using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    private void Awake()
    {
        instance = this;
    }

    // List of all the global stats
    public float researchSpeed = 1;
    public float agreeblness = 1; // Something like charisma, makes countries more likely to agree with you
}
