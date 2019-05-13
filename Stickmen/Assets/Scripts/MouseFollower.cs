using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {

    public bool stick_to_ground = false;
    public bool stick_to_platform = false;
    public bool border_mode = false;
    public float snap_distance = 0f;
    public float border_dist = 1.5f;
    public GameObject stickman;
    public bool Switchable = true;

    private Vector3 Last_position;
    private Vector3 Last_position_border;
    private bool isOnGround = true;
    private bool lookingRight = false;

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
        else if (snap_distance != 0 && snap_distance >= DistanceToGround())
        {
            MoveToGround();
        }
        else
        {
            Last_position = transform.position;
            SwitchGroundState(false);
        }

        UpdateOrientation();

        if (Input.GetKeyDown(KeyCode.Space) && Switchable)
        {
            if (stickman.GetComponent<NetworkPlayer>())
            {
                stickman.GetComponent<NetworkPlayer>().SwitchAimer();
            }
            if (stickman.GetComponent<Shade>())
            {
                stickman.GetComponent<Shade>().SwitchAimer();
            }
        }
    }

    private void MoveToGround()
    {
        Vector3 starting_pos = transform.position;
        Platform platform = PlatformManager.instance.GetCurrentPlatform();
        SwitchGroundState(true);

        int layerMask = 1 << 11;

        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            if (stick_to_platform)
            {
                if(CheckBorders())
                {
                    Last_position_border = hit.point + new Vector3(0f, 0.02f, 0f);
                }
                if(border_mode)
                {
                    transform.position = Last_position_border;
                    Last_position = transform.position;
                }
                else if(!border_mode && Mathf.Abs(hit.point.y - Last_position.y) < 2 && hit.point.x < platform.x_max && hit.point.x > platform.x_min)
                {
                    transform.position = hit.point + new Vector3(0f, 0.02f, 0f);
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
                transform.position = hit.point + new Vector3(0f, 0.02f, 0f);
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

    private bool CheckBorders()
    {
        Platform platform = PlatformManager.instance.GetCurrentPlatform();
        int layerMask = 1 << 11;

        RaycastHit hit_right;
        if (Physics.Raycast(transform.position + new Vector3(border_dist, 2, 0), transform.TransformDirection(Vector3.down), out hit_right, Mathf.Infinity, layerMask))
        {
            if (Mathf.Abs(hit_right.point.y - Last_position.y) < 2 && hit_right.point.x < platform.x_max && hit_right.point.x > platform.x_min)
            {
                RaycastHit hit_left;
                if (Physics.Raycast(transform.position + new Vector3(-border_dist, 2, 0), transform.TransformDirection(Vector3.down), out hit_left, Mathf.Infinity, layerMask))
                {
                    if (Mathf.Abs(hit_left.point.y - Last_position.y) < 2 && hit_left.point.x < platform.x_max && hit_left.point.x > platform.x_min)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private float DistanceToGround()
    {
        Vector3 starting_pos = transform.position;
        Platform platform = PlatformManager.instance.GetCurrentPlatform();

        int layerMask = 1 << 11;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            return starting_pos.y - hit.point.y;
        }
        else
        {
            return 10f;
        }
    }

    public bool GetIsOnGround()
    {
        return isOnGround;
    }

    public void SetIsOnGround(bool onGround)
    {
        isOnGround = onGround;
    }

    public bool GetLookingRight()
    {
        return lookingRight;
    }

    public void SetLookingRight(bool lRight)
    {
        lookingRight = lRight;
    }

    public Vector3 GetLastBorderPos()
    {
        return Last_position_border;
    }

    public void SetLastBorderPos(Vector3 pos)
    {
        Last_position_border = pos;
    }

    private void UpdateOrientation()
    {
        if (stickman != null)
        {
            if (transform.position.x > stickman.transform.position.x)
            {
                if (!lookingRight)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    lookingRight = true;
                }
            }
            else
            {
                if (lookingRight)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    lookingRight = false;
                }
            }
        }
    }

    private void SwitchGroundState(bool onGround)
    {
        if(isOnGround != onGround)
        {
            isOnGround = onGround;
            if (stickman.GetComponent<NetworkPlayer>())
            {
                stickman.GetComponent<NetworkPlayer>().SwitchAimer();
            }
            if (stickman.GetComponent<Shade>())
            {
                stickman.GetComponent<Shade>().SwitchAimer();
            }
        }
    }
}
