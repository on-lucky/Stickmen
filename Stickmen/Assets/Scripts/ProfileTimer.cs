using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileTimer : MonoBehaviour {

    public static ProfileTimer instance;

    public int _secondsToCount = 120;

    private float _currentTimer;
    private TextMeshPro _textMesh;
    private bool _isCounting = false;

    private void Awake()
    {
        if (ProfileTimer.instance != null)
        {
            Debug.LogError("More than one ProfileTimer in the scene!");
        }
        ProfileTimer.instance = this;
    }

    void Start () {
        _textMesh = GetComponentInChildren<TextMeshPro>();
        _currentTimer = _secondsToCount;
        SetTimerText();
	}
	
	void Update () {
        if (_isCounting)
        {
            UpdateTimer();
        }
	}

    private void UpdateTimer()
    {
        _currentTimer -= Time.deltaTime;
        if (_currentTimer <= 0)
        {
            _currentTimer = 0;
            _isCounting = false;
            ForceGameStart();
        }
        SetTimerText();
    }

    private void SetTimerText()
    {
        int minutes = (int)_currentTimer / 60;
        int seconds = (int)_currentTimer % 60;
        if(seconds < 10)
        {
            _textMesh.text = minutes + " : 0" + seconds;
        }
        else
        {
            _textMesh.text = minutes + " : " + seconds;
        }
    }

    public void StartTimer()
    {
        _isCounting = true;
    }

    private void ForceGameStart()
    {
        //TODO: forces the start of the game
    }
}
