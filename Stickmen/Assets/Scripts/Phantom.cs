using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour {

    public StickmanProfile _profile;
    public float fading_speed = 2f;

    private Vector3 goal;
    private Material mat;
    private float alpha;

    private void Start()
    {
        GetComponent<Rigidbody>().useGravity = true;
        goal = GameManager.instance.target.transform.position;
        mat = GetComponentInChildren<Renderer>().materials[0];
        alpha = mat.color.a;
    }

    private void Update()
    {
        if (GoalReached() || mat.color.a <= 0)
        {
            //Destroy(this.gameObject);
        }
        else
        {
            alpha = Mathf.Max(alpha - (Time.deltaTime * fading_speed), 0);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
        }
    }

    private bool GoalReached()
    {
        if(Mathf.Abs(goal.x - transform.position.x) > 0.5)
        {
            return false;
        }
        else if(Mathf.Abs(goal.y - transform.position.y) > 0.5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetProfile(StickmanProfile profile)
    {
        _profile = profile;
    }
}
