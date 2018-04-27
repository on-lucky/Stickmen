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

	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            if (runner.transform.position.x < goal_position.x)
            {
                if (goal_position.x - runner.transform.position.x > 1)
                {
                    if (CheckSpeed())
                    {
                        Vector3 direction = new Vector3((Mathf.Cos(ground_angle) * stickman_acceleration), (Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
                else
                {
                    //break
                    if (runner.GetComponent<Rigidbody>().velocity.x > 0)
                    {
                        Vector3 direction = new Vector3(-stickman_acceleration, 0, 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
            }
            if (runner.transform.position.x > goal_position.x)
            {
                if (runner.transform.position.x - goal_position.x > 1)
                {
                    if (CheckSpeed())
                    {
                        Vector3 direction = new Vector3(-(Mathf.Cos(ground_angle) * stickman_acceleration), -(Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
                else
                {
                    //break
                    if (runner.GetComponent<Rigidbody>().velocity.x < 0)
                    {
                        Vector3 direction = new Vector3(stickman_acceleration, 0, 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
            }
            if (runner != null)
            {
                animator.SetFloat("Speed", Mathf.Abs(runner.GetComponent<Rigidbody>().velocity.x));
                if (runner.GetComponent<Rigidbody>().velocity.x == 0 && !IsAtStart())
                {
                    isRunning = false;
                    if (!isPhantom)
                    {
                        GameManager.instance.InitChoosingMode();
                    }
                }
            }
        }

        if (isWalking)
        {
            if (runner.transform.position.x < goal_position.x)
            {
                if (goal_position.x - runner.transform.position.x > 0.4)
                {
                    if (CheckSpeed())
                    {
                        Vector3 direction = new Vector3((Mathf.Cos(ground_angle) * stickman_acceleration), (Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
            }
            if (runner.transform.position.x > goal_position.x)
            {
                if (runner.transform.position.x - goal_position.x > 0.4)
                {
                    if (CheckSpeed())
                    {
                        Vector3 direction = new Vector3(-(Mathf.Cos(ground_angle) * stickman_acceleration), -(Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                        runner.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                    }
                }
            }
            if (runner != null)
            {
                animator.SetFloat("Speed", Mathf.Abs(runner.GetComponent<Rigidbody>().velocity.x));
                if (runner.GetComponent<Rigidbody>().velocity.x == 0 && !IsAtStart())
                {
                    isWalking = false;
                    if (!isPhantom)
                    {
                        GameManager.instance.InitChoosingMode();
                    }
                }
            }
        }
    }

    public void RunTo(Vector3 goal, GameObject new_runner)
    {
        goal_position = goal;
       
        runner = new_runner;

        starting_position = runner.transform.position;
        if (Mathf.Abs(goal_position.x - runner.transform.position.x) > 2)
        {
            isRunning = true;
        }
        else
        {
            isWalking = true;
        }
        animator = runner.GetComponentInChildren<Animator>();
        if (runner.transform.position.x < goal_position.x)
        {
            GetComponentInChildren<OrientationManager>().LookRight();
        }
        else
        {
            GetComponentInChildren<OrientationManager>().LookLeft();
        }
    }

    private bool IsAtStart()
    {
        return (Mathf.Abs(runner.transform.position.x - starting_position.x) < 0.1f);
    }

    private bool CheckSpeed()
    {
        return (Mathf.Abs(runner.GetComponent<Rigidbody>().velocity.x) < stickman_speed);
    }

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
