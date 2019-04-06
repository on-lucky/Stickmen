using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : MonoBehaviour {

    public bool isRight = true;

	public void LookLeft()
    {
        if (isRight)
        {
            isRight = false;
            TurnAnim();
        }
    }

    public void LookRight()
    {
        if (!isRight)
        {
            isRight = true;
            TurnAnim();
        }
    }

    public void LookTo(bool shouldLookRight)
    {
        if (shouldLookRight)
        {
            LookRight();
        }
        else
        {
            LookLeft();
        }
    }

    private void TurnAnim()
    {
        GetComponent<Animator>().SetTrigger("turn");
    }
}
