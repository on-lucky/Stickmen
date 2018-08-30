using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

    public Light[] lights;                  // List of lignts affected by the LightSwitch 
    public float intensity = 1;             // Base intensity of the lights
    public float goalIntensity =1;          // goal intensity after the flash
    public float flashSpeed = 5;            // Speed of the flash
    public float flashIntensity = 3;        // Intensity multiplier during the flash

    private bool isflashing = false;        // If the lights are flashing

	// Use this for initialization
	void Start () {
        LightDOWN();
        ResetIntensity();
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
            light.intensity = intensity * flashIntensity;
        }
        isflashing = true;
    }

    /// <summary>
    /// Brings back the intensity of the lights to the appropriate one
    /// </summary>
    private void UpdateIntensity()
    {
        if (isflashing)
        {
            foreach (Light light in lights)
            {
                if (light.intensity > goalIntensity)
                {
                    light.intensity -= (Time.deltaTime * intensity * flashSpeed);
                }
                else
                {
                    isflashing = false;
                }
            }
        }
    }

    /// <summary>
    /// Resets the intensity value of each lights to the value of the lightswitch intensity
    /// </summary>
    private void ResetIntensity()
    {
        foreach (Light light in lights)
        {
            light.intensity = intensity;
        }
    }
}
