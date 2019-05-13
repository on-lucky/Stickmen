using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    All,
    Iddle,      // stateIndex = 0 in the animator
    Jump,       // stateIndex = 1 in the animator
    Run         // stateIndex = 2 in the animator
}

public class AnimationManager : MonoBehaviour {

    public AnimState aState = AnimState.Iddle;

    private Animator animator;

    private float verticalSpeed = 0f;
    private float horisontalSpeed = 0f;
    private Vector3 lastPos;
    private bool lookingRight = true;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        lastPos = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateVSpeed(Time.deltaTime);

		switch (aState)
        {
            case AnimState.Iddle:
                break;
            case AnimState.Jump:
                animator.SetFloat("VerticalSpeed", verticalSpeed);
                break;
            case AnimState.Run:
                animator.SetFloat("Speed", horisontalSpeed);
                break;
        }
    }

    public void SwitchState(AnimState newState)
    {
        aState = newState;

        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        switch (aState)
        {
            case AnimState.Iddle:
                animator.SetInteger("stateIndex", 0);
                break;
            case AnimState.Jump:
                animator.SetTrigger("Jump");
                break;
            case AnimState.Run:
                animator.SetInteger("stateIndex", 2);
                break;
        }
    }

    private void UpdateVSpeed(float deltaTime)
    {
        Vector3 newPos = this.transform.position;
        verticalSpeed = (newPos.y - lastPos.y) / deltaTime;
        lastPos = newPos;
    }

    public void SetHSpeed(float hSpeed)
    {
        horisontalSpeed = hSpeed;
    }

    public float GetHSpeed()
    {
        return horisontalSpeed;
    }

    public void SetLookingRight(bool lRight)
    {
        lookingRight = lRight;
    }

    public bool GetLookingRight()
    {
        return lookingRight;
    }
}
