using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAnimator : MonoBehaviour {

    private Animator _animator;
    private Rigidbody _rigidBody;

	// Use this for initialization
	void Start () {
        _animator = GetComponentInChildren<Animator>();
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
	
	// Update is called once per frame
	void Update () {
        float speed = _rigidBody.velocity.magnitude;
        _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
	}
}
