using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Vector3 _StartingDirection = new Vector3(0, 1, 0);        // Direction in which the projectile is going at the start
    public float _force = 5;                                         // Starting force aplied to the projectile

    protected Rigidbody _rigidbody;

    virtual protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    virtual protected void Start () {
        _StartingDirection = _StartingDirection.normalized;
        Throw();
	}

    /// <summary>
    /// Throws the projectile in the direction of _StartingDirection with adequate force
    /// </summary>
    private void Throw()
    {
        Vector3 throwVector = _force * _StartingDirection;
        _rigidbody.AddForce(throwVector);
    }
}
