using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new horizontal movement move", menuName = "horizontal movement move")]
public class HorizontalMovementMove : PureMovementMove
{
    public float speed_multiplier = 1f;
    public float acceleration_multiplier = 2f;

    private float stickman_speed;
    private float stickman_acceleration;

    public void Init(GameObject _stickman, Vector3 startingPos, Vector3 goalPos)
    {
        stickman = _stickman;
        starting_position = startingPos;
        goal_position = goalPos;
        animator = stickman.GetComponentInChildren<Animator>();
        stickman_speed = GetProfile().GetStatValue(Stats.dexterity) * speed_multiplier;
        stickman_acceleration = GetProfile().GetStatValue(Stats.dexterity) * acceleration_multiplier;
        SetIsPhantom();
    }

    public override void SetUp(GameObject _stickman)
    {
        base.SetUp(_stickman);
        stickman_speed = GetProfile().GetStatValue(Stats.dexterity) * speed_multiplier;
        stickman_acceleration = GetProfile().GetStatValue(Stats.dexterity) * acceleration_multiplier;
        target.GetComponent<MouseFollower>().stick_to_platform = true;
    }

    public override void SpawnPhantom(MouseFollower target)
    {
        GameObject phantom = Instantiate(GameManager.instance.phantom_template, stickman.transform);

        HorizontalMovementMove phantomMove = ScriptableObject.CreateInstance<HorizontalMovementMove>();
        phantomMove.Init(phantom, stickman.transform.position, target.transform.position);

        ShadeMoveManager.instance.AddMove(phantomMove);

        phantomMove.PhantomExecute();
    }

    public override bool UpdateMethod(float deltaTime)
    {
        bool isMoveOver = false;
        float ground_angle = stickman.GetComponent<StickmanRunner>().ground_angle;
        if (stickman.transform.position.x < goal_position.x)
        {
            if (goal_position.x - stickman.transform.position.x > 1)
            {
                if (CheckSpeed())
                {
                    Vector3 direction = new Vector3((Mathf.Cos(ground_angle) * stickman_acceleration), (Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                    stickman.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                }
            }
            else
            {
                if (stickman.GetComponent<Rigidbody>().velocity.x > 0)
                {
                    Vector3 direction = new Vector3(-stickman_acceleration, 0, 0);
                    stickman.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                }
            }
        }
        if (stickman.transform.position.x > goal_position.x)
        {
            if (stickman.transform.position.x - goal_position.x > 1)
            {
                if (CheckSpeed())
                {
                    Vector3 direction = new Vector3(-(Mathf.Cos(ground_angle) * stickman_acceleration), -(Mathf.Sin(ground_angle) * stickman_acceleration), 0);
                    stickman.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                }
            }
            else
            {
                if (stickman.GetComponent<Rigidbody>().velocity.x < 0)
                {
                    Vector3 direction = new Vector3(stickman_acceleration, 0, 0);
                    stickman.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
                }
            }
        }
        if (stickman != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(stickman.GetComponent<Rigidbody>().velocity.x));
            if (Mathf.Abs(stickman.GetComponent<Rigidbody>().velocity.x) < 0.1 && !IsAtStart())
            {
                animator.speed = 0;
                ShadeMoveManager.instance.RemoveMove(this);
                isMoveOver = true;
                if (!isPhantom)
                {
                    GameManager.instance.InitChoosingMode();
                }
                else
                {
                    Destroy(stickman);
                }
            }
        }
        return isMoveOver;
    }

    private bool CheckSpeed()
    {
        return (Mathf.Abs(stickman.GetComponent<Rigidbody>().velocity.x) < stickman_speed);
    }

    private bool IsAtStart()
    {
        return (Mathf.Abs(stickman.transform.position.x - starting_position.x) < 0.1f);
    }
}
