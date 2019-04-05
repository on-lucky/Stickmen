using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanRunner : MonoBehaviour {

    private bool isRunning = false;
    private bool isWalking = false;

    public Vector3 goal_position;
    public float stickman_speed = 3f;
    public float stickman_acceleration = 8f;
    public float ground_angle;
    public bool isPhantom = false;

    private Vector3 starting_position;
    private GameObject runner;
    private Animator animator;
    private int collision_counter = 0;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11){
            collision_counter++;
            ground_angle = other.transform.eulerAngles.z * Mathf.Deg2Rad;
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 11)
        {
            collision_counter--;
            if (collision_counter == 0)
            {
                if (GetComponentInChildren<OrientationManager>().isRight)
                {
                    ground_angle = 270 * Mathf.Deg2Rad;
                }
                else
                {
                    ground_angle = 90 * Mathf.Deg2Rad;
                }
            }
        }
    }
}
