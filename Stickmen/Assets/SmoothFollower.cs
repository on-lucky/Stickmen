using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollower : MonoBehaviour {

    public Transform objective;
    public float moveSpeed = 1;
    public float breakDistance = 2;

    private float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Distance();
        FollowObjective();
	}

    private void FollowObjective()
    {
        if (distance > 0.001)
        {
            MoveCam();
        }
    }

    private void Distance()
    {
        distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - objective.position.x, 2) + 
                                    Mathf.Pow(transform.position.y - objective.position.y, 2));
    }

    private void MoveCam()
    {
        Vector3 direction = objective.position - transform.position;
        direction.z = 0;
        direction = direction.normalized;

        if(distance < breakDistance)
        {
            transform.Translate(direction * moveSpeed * (distance / breakDistance));
        }
        else
        {
            transform.Translate(direction * moveSpeed);
        }
        
    }
}
