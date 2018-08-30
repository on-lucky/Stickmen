using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    public static TimeController instance;

    private List<TimeModule> rewindableElements;

    private void Awake()
    {
        if (TimeController.instance != null)
        {
            Debug.LogError("More than one TimeController in the scene!");
        }
        TimeController.instance = this;
    }

    // Use this for initialization
    void Start () {
        rewindableElements = new List<TimeModule>();
	}
	
	public void AddRewindableElement(TimeModule element)
    {
        rewindableElements.Add(element);
    }

    public void RewindTo(float ratio)
    {
        foreach(TimeModule element in rewindableElements)
        {
            element.RewindTo(ratio);
        }
    }

    public void StartRewind()
    {
        foreach (TimeModule element in rewindableElements)
        {
            element.StartRewind();
        }
    }

    public void StopRewind()
    {
        foreach (TimeModule element in rewindableElements)
        {
            element.StopRewind();
        }
    }

    public void StartRecording()
    {
        foreach (TimeModule element in rewindableElements)
        {
            element.StartRecording();
        }
    }

    public void StopRecording()
    {
        foreach (TimeModule element in rewindableElements)
        {
            element.StopRecording();
        }
    }
}
