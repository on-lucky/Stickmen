using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {

    public bool stick_to_ground = false;
    public bool stick_to_platform = false;

    private Vector3 Last_position;

    // Use this for initialization
    void Start () {
        Last_position = transform.position;
        GameManager.instance.target = this;
    }
	
	// Update is called once per frame
	void Update () {

        if(GameManager.instance.game_state != GameManager.GameState.Aiming)
        {
            GameManager.instance.local_shade.GetComponent<PhantomSpawner>().StopSpawning();
            Destroy(this.gameObject);
        }

		Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.z = 0;
        transform.position = mouseWorldPos;
        if (stick_to_ground || stick_to_platform)
        {
            MoveToGround();
        }
        else
        {
            Last_position = transform.position;
        }
    }

    private void MoveToGround()
    {
        Vector3 starting_pos = transform.position;
        Platform platform = PlatformManager.instance.GetCurrentPlatform();

        int layerMask = 1 << 11;

        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            if (stick_to_platform)
            {
                
                if(Mathf.Abs(hit.point.y - Last_position.y) < 2 && hit.point.x < platform.x_max && hit.point.x > platform.x_min)
                {
                    transform.position = hit.point;
                    Last_position = transform.position;
                }
                else
                {
                    if (starting_pos.x < platform.x_max && starting_pos.x > platform.x_min)
                    {
                        Vector3 new_pos = new Vector3(hit.point.x, Last_position.y, Last_position.z);
                        transform.position = new_pos;
                    }
                    else
                    {
                        transform.position = Last_position;
                    }
                    Last_position = transform.position;
                }
            }
            else
            {
                transform.position = hit.point;
                Last_position = transform.position;
            }
        }
        else
        {
            if (starting_pos.x < platform.x_max && starting_pos.x > platform.x_min)
            {
                Vector3 new_pos = new Vector3(starting_pos.x, Last_position.y, Last_position.z);
                transform.position = new_pos;
            }
            else
            {
                transform.position = Last_position;
            }
            Last_position = transform.position;
        }
    }
}
