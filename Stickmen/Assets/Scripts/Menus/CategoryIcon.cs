using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryIcon : Icon {

    public List<Icon> childIcons;

    protected override void OnMouseDown()
    {
        foreach (var icon in childIcons)
        {
            icon.Appear();
        }
        _parent.TakeChildrenText();
    }

    public  override void TakeChildrenText()
    {
        foreach (Icon icon in childIcons)
        {
            icon.RemoveText();
        }
    }

    public override void Addchild(Icon icon)
    {
        if(childIcons == null)
        {
            childIcons = new List<Icon>();
        }
        childIcons.Add(icon);
    }
}
