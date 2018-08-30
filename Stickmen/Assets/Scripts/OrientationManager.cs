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
            transform.Rotate(Vector3.up, 180);
        }
    }

    public void LookRight()
    {
        if (!isRight)
        {
            isRight = true;
            transform.Rotate(Vector3.up, 180);
        }
    }
}
