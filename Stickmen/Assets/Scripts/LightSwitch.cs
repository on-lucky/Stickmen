using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    public Light[] lights;                  // List of lignts affected by the LightSwitch 
    public float intensity = 1;             // Base intensity of the lights

    private const float FLASHSPEED = 5;     // Speed of the flash

	// Use this for initialization
	void Start () {
        LightDOWN();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateIntensity();
	}

    /// <summary>
    /// Turns ON the lights 
    /// </summary>
    public void LightUP()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    /// <summary>
    /// Turns OFF the lights
    /// </summary>
    public void LightDOWN()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

    /// <summary>
    /// Makes the lights flash
    /// </summary>
    public void Flash()
    {
        foreach (Light light in lights)
        {
            light.intensity = intensity * 3;
        }
    }

    /// <summary>
    /// Brings back the intensity of the lights to the appropriate one
    /// </summary>
    private void UpdateIntensity()
    {
        foreach (Light light in lights)
        {
            if (light.intensity > intensity)
            {
                light.intensity -= (Time.deltaTime * intensity * FLASHSPEED);
            }
        }
    }
}
