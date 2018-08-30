using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public float rotationSpeed = 1f;
    public Vector3 translationSpeed = new Vector3(1f, 0, 0);
    public float spawningTime = 1f;
    public GameObject fireTrail;

    private float currentTime;

	// Use this for initialization
	void Start () {
        currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        if(currentTime > spawningTime)
        {
            SpawnFire();
            currentTime = 0;
        }

        Move();
        Rotate();
	}

    private void SpawnFire()
    {
        if (fireTrail)
        {
            Instantiate(fireTrail, transform.position, transform.rotation);
        }
    }

    private void Rotate()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void Move()
    {
        transform.Translate(translationSpeed, Space.World);
    }
}
