using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public static Timer instance;
    public float timeToWait;


    private float currentTime;
    private bool counting = false;

    private void Awake()
    {
        if (Timer.instance != null)
        {
            Debug.LogError("More than one Timer in the scene!");
        }
        Timer.instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (counting)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= timeToWait)
            {
                
            }
        }
	}
}
