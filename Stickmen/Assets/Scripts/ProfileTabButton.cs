using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileTabButton : ButtonSelector {

    public MenuElement[] AssociatedObjects;

    private bool selected = false;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (!selected)
        {
            selected = true;
            foreach (MenuElement elem in AssociatedObjects)
            {
                elem.Appear();
            }
        }
    }
}
