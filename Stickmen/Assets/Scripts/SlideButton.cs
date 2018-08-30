using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton : ButtonSelector {

    private bool isRight = true;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if (isRight)
        {
            SideBarScaler.instance.Slide(false);
            isRight = false;
        }else{
            SideBarScaler.instance.Slide(true);
            isRight = true;
        }
        
    }
}
