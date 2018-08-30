using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public GameObject subject;

    void Update()
    {
        if (subject != null)
        {
            transform.LookAt(subject.transform.position,
            Vector3.up);
        }
    }
}
