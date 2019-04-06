using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    Iddle,
    Jump,
    Run
}

public class AnimationManager : MonoBehaviour {

    public AnimState aState = AnimState.Iddle;

    private Animator animator;

    private float verticalSpeed = 0f;
    private Vector3 lastPos;

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
                break;
            case AnimState.Jump:
                animator.SetTrigger("Jump");
                break;
            case AnimState.Run:
                break;
        }
    }

    private void UpdateVSpeed(float deltaTime)
    {
        Vector3 newPos = this.transform.position;
        verticalSpeed = (newPos.y - lastPos.y) / deltaTime;
        lastPos = newPos;
    }
}
