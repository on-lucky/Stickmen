using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChangerButton : ButtonSelector
{

    public int goalScreenIndex;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        StartCoroutine(MenuManager.instance.ChangeScreen(goalScreenIndex));
    }
}
