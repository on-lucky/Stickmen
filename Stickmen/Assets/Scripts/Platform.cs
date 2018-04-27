using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform {

    //public BoxCollider _collider;

    public float x_min;
    public float x_max;
    public float y_top;

    /*public void Init(GameObject reference_collider)
    {
        
        _collider = new BoxCollider
        {
            size = new Vector3(x_max - x_min, 1, 10),
            center = new Vector3((x_max + x_min) / 2, y_top - 0.5f, 0)
        };
    }*/
}
