using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : ButtonSelector {

    private bool isPressed = false;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if (!isPressed)
        {
            isPressed = true;
            NetworkMessenger.instance.SendReadyToStartMessage();
        }
    }
    }
