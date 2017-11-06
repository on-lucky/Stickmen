using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour {

    public static PowerManager instance;

    public Power[] allPowers;

    private void Awake()
    {
        if (PowerManager.instance != null)
        {
            Debug.LogError("More than one PowerManager in the scene!");
        }
        PowerManager.instance = this;
    }
}
