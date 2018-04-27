using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour {

    public GameObject footstep;

	public void FootStep()
    {
        Vector3 foot_position;

        if (GetComponent<OrientationManager>().isRight)
        {
            foot_position = transform.position + new Vector3(0.5f, 0, 0);
        }
        else
        {
            foot_position = transform.position - new Vector3(0.5f, 0, 0);
        }
        GameObject fs = Instantiate(footstep, foot_position, transform.rotation);
        Destroy(fs, 1f);
    }
}
