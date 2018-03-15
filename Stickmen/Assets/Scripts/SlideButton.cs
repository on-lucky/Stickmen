using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton : ButtonSelector {

    private bool isRight = false;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if (isRight)
        {
            SideBarScaler.instance.SlideRight(false);
            isRight = false;
        }else{
            SideBarScaler.instance.SlideRight(true);
            isRight = true;
        }
        
    }
}
