using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new horizontal movement move", menuName = "horizontal movement move")]
public class HorizontalMovementMove : PureMovementMove
{
    public float speed_multiplier = 1f;
    public float acceleration_multiplier = 0.1f;

    private float stickman_speed;
    private float stickman_acceleration;
    private Vector3 lastPos;
    private float current_speed = 0f;

    public void Init(GameObject _stickman, Vector3 startingPos, Vector3 goalPos)
    {
        stickman = _stickman;
        starting_position = startingPos;
        goal_position = goalPos;
        animator = stickman.GetComponentInChildren<Animator>();
        stickman_speed = GetProfile().GetStatValue(Stats.dexterity) * speed_multiplier;
        stickman_acceleration = GetProfile().GetStatValue(Stats.dexterity) * acceleration_multiplier;
        SetIsPhantom();
        lastPos = stickman.transform.position;
        current_speed = 0f;
    }

    public override void SetUp(GameObject _stickman)
    {
        base.SetUp(_stickman);
        stickman_speed = GetProfile().GetStatValue(Stats.dexterity) * speed_multiplier;
        stickman_acceleration = GetProfile().GetStatValue(Stats.dexterity) * acceleration_multiplier;
        lastPos = _stickman.transform.position;
        current_speed = 0f;
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
                if (CheckSpeed(deltaTime))
                {
                    Accelerate();
                    
                }
                Vector3 direction = new Vector3((Mathf.Cos(ground_angle) * current_speed * deltaTime), (Mathf.Sin(ground_angle) * current_speed * deltaTime), 0);
                stickman.transform.position = stickman.transform.position + direction;
            }
            else
            {
                Decelerate();
                Vector3 direction = new Vector3(current_speed * deltaTime, 0, 0);
                stickman.transform.position = stickman.transform.position + direction;
               
            }
        }
        if (stickman.transform.position.x > goal_position.x)
        {
            if (stickman.transform.position.x - goal_position.x > 1)
            {
                if (CheckSpeed(deltaTime))
                {
                    Accelerate();
                }
                Vector3 direction = new Vector3(-(Mathf.Cos(ground_angle) * current_speed * deltaTime), -(Mathf.Sin(ground_angle) * current_speed * deltaTime), 0);
                stickman.transform.position = stickman.transform.position + direction;
            }
            else
            { 
                Decelerate();
                Vector3 direction = new Vector3(-current_speed * deltaTime, 0, 0);
                stickman.transform.position = stickman.transform.position + direction;
            }
        }
        if (stickman != null)
        {
            animator.SetFloat("Speed", current_speed);
            if (current_speed < 0.1 && !IsAtStart())
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

    private void Accelerate()
    {
        current_speed += stickman_acceleration;
        Mathf.Clamp(current_speed, 0, stickman_speed);
    }

    private void Decelerate()
    {
        current_speed -= stickman_acceleration;
        Mathf.Clamp(current_speed, 0, stickman_speed);
    }

    private bool CheckSpeed(float deltaTime)
    {
        //Vector3 newPos = stickman.transform.position;
        //float speed = (newPos.x - lastPos.x) / deltaTime;
        return (current_speed < stickman_speed);

    }

    private bool IsAtStart()
    {
        return (Mathf.Abs(stickman.transform.position.x - starting_position.x) < 0.1f);
    }

    protected override void SetTarget(int index, GameObject sMan)
    {
        base.SetTarget(index, sMan);
        target.GetComponent<MouseFollower>().stick_to_platform = true;
    }
}
