using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarSlider : MonoBehaviour {

    private const float TIME_HOP = 0.05f;

    public static TimeBarSlider instance;

    public float minPosX = 0;
    public float maxPosX = 0;

    public float minCursorPosX = 0;
    public float maxCursorPosX = 0;

    public TimeCursor timeCursor;

    public float timeHop = TIME_HOP;
    public float minimumKeyHold = 1;

    private bool isCounting = false;
    private float turnDuration = 0;
    private float currentTime = 0;
    private float cursorTime = 0;

    private float currentHoldTime = 0;
    private Stack<float> stepTimes = new Stack<float>();

    private void Awake()
    {
        if (TimeBarSlider.instance != null)
        {
            Debug.LogError("More than one TimeBarSlider in the scene!");
        }
        TimeBarSlider.instance = this;
        stepTimes.Push(0);
    }

    // Update is called once per frame
    void Update () {
        UpdateBar();
        HandleKeys();
    }

    private void HandleKeys ()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HopInTime(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HopInTime(false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime > minimumKeyHold)
            {
                HopInTime(true, Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime > minimumKeyHold)
            {
                HopInTime(false, Time.deltaTime);
            }
        }
        else
        {
            currentHoldTime = 0;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                TakeStep(false);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                TakeStep(true);
            }
        }
    }

    private void UpdateBar()
    {
        if (isCounting)
        {
            currentTime += Time.deltaTime;
            cursorTime = currentTime;
            PlaceBar();
            PlaceCursor(currentTime);
            timeCursor.UpdateLimits();
            if (currentTime >= turnDuration)
            {
                currentTime = turnDuration;
                isCounting = false;
            }
        }
    }

    private void PlaceBar()
    {
        float posX = minPosX + ((maxPosX - minPosX) * (currentTime / turnDuration));
        transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
    }

    private void PlaceCursor(float time)
    {
        float posX = minCursorPosX + ((maxCursorPosX - minCursorPosX) * (time / turnDuration));
        timeCursor.PlaceCursor(posX); 
    }

    public void SetDuration(float duration)
    {
        turnDuration = duration;
    }

    public void StartCounting()
    {
        isCounting = true;
        MatchBarToCursor();
        UpdateStepsToTime(currentTime);
        stepTimes.Push(currentTime);
        TimeController.instance.StartRecording();
    }

    public void StopCounting()
    {
        isCounting = false;
        TimeController.instance.StopRecording();
    }

    public void UpdateCursorTime (float ratio)
    {
        cursorTime = ratio * currentTime;
    }

    private void MatchBarToCursor()
    {
        float cursorRatio = (timeCursor.transform.localPosition.x - minCursorPosX) / (maxCursorPosX - minCursorPosX);
        currentTime = cursorRatio * turnDuration;
    }

    public void HopInTime(bool forward, float timeToHop = TIME_HOP)
    {
        float time = forward ? timeToHop : -timeToHop;

        cursorTime = Mathf.Clamp(cursorTime + time, 0, currentTime);
        
        PlaceCursor(cursorTime);
        timeCursor.ApplyRewind();
    }

    public void TakeStep(bool forward)
    {
        float tempTime = forward? currentTime : 0f;

        foreach(float stepTime in stepTimes)
        {
            if (forward && stepTime > cursorTime)
            {
                tempTime = stepTime;
            }
            else if(!forward && stepTime < cursorTime)
            {
                tempTime = stepTime;
                break;
            }
        }
        cursorTime = tempTime;
        PlaceCursor(cursorTime);
        timeCursor.ApplyRewind();
    }

    private void UpdateStepsToTime(float time)
    {
        bool finished = false;
        while (!finished)
        {
            if (stepTimes.Peek() > time)
            {
                stepTimes.Pop();
            }
            else
            {
                finished = true;
            }
        }
    }
}
