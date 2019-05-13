using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new horizontal movement move", menuName = "horizontal movement move")]
public class HorizontalMovementMove : PureMovementMove
{
    public float speed_multiplier = 1f;
    public float acceleration_multiplier = 0.1f;
    public float distForBreak = 1f;

    private float stickman_speed;
    private float stickman_acceleration;
    private Vector3 lastPos;
    private float current_speed = 0f;
    public float minSpeed = 0.4f;

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
        SetInitialSpeed();
    }

    public override void SetUp(GameObject _stickman)
    {
        base.SetUp(_stickman);
        stickman_speed = GetProfile().GetStatValue(Stats.dexterity) * speed_multiplier;
        stickman_acceleration = GetProfile().GetStatValue(Stats.dexterity) * acceleration_multiplier;
        lastPos = _stickman.transform.position;
        //SetInitialSpeed();
    }

    public override void SpawnPhantom(MouseFollower target)
    {
        GameObject phantom = Instantiate(GameManager.instance.phantom_template, stickman.transform);

        HorizontalMovementMove phantomMove = ScriptableObject.CreateInstance<HorizontalMovementMove>();
        phantomMove.Init(phantom, stickman.transform.position, target.transform.position);

        ShadeMoveManager.instance.AddMove(phantomMove);

        phantomMove.PhantomExecute();
    }

    public override void PhantomExecute()
    {
        base.PhantomExecute();
        stickman.GetComponent<AnimationManager>().SwitchState(AnimState.Run);
    }

    public override bool UpdateMethod(float deltaTime)
    {
        bool isMoveOver = false;
        float ground_angle = stickman.GetComponent<StickmanRunner>().ground_angle;
        Debug.Log(ground_angle);
        if (stickman.transform.position.x < goal_position.x)
        {
            if (aimerIndex == 1 || (goal_position.x - stickman.transform.position.x > distForBreak || current_speed < minSpeed))
            {
                if (CheckSpeed())
                {
                    Accelerate();
                }
                Vector3 direction = new Vector3((Mathf.Cos(ground_angle) * current_speed * deltaTime), (Mathf.Sin(ground_angle) * current_speed * deltaTime), 0);
                //Debug.Log(direction);
                stickman.transform.position = stickman.transform.position + direction;
            }
            else
            {
                Decelerate();
                Vector3 direction = new Vector3(current_speed * deltaTime, 0, 0);
                stickman.transform.position = stickman.transform.position + direction;
            }
            if (stickman != null)
            {
                //animator.SetFloat("Speed", current_speed);
                UpdateAnimator();
                if (stickman.transform.position.x > goal_position.x || (current_speed == 0 && !IsAtStart()))
                {
                    StopRunning(true);
                    isMoveOver = true;
                }
            }
        }
        if (stickman.transform.position.x > goal_position.x)
        {
            if (aimerIndex == 1 || (stickman.transform.position.x - goal_position.x > distForBreak || current_speed < minSpeed))
            {
                if (CheckSpeed())
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
            if (stickman != null)
            {
                //animator.SetFloat("Speed", current_speed);
                UpdateAnimator();
                if (stickman.transform.position.x < goal_position.x || (current_speed == 0 && !IsAtStart()))
                {
                    StopRunning(false);
                    isMoveOver = true;
                }
            }
        }
        
        return isMoveOver;
    }

    private void Accelerate()
    {
        current_speed += stickman_acceleration;
        current_speed = Mathf.Clamp(current_speed, -1000, stickman_speed);
    }

    private void Decelerate()
    {
        current_speed -= stickman_acceleration;
        current_speed = Mathf.Clamp(current_speed, minSpeed, stickman_speed);
    }

    private bool CheckSpeed()
    {
        return (current_speed < stickman_speed);

    }

    private bool IsAtStart()
    {
        return (Mathf.Abs(stickman.transform.position.x - starting_position.x) < 1.5f);
    }

    protected override void SetTarget(int index, GameObject sMan)
    {
        base.SetTarget(index, sMan);
        target.GetComponent<MouseFollower>().stick_to_platform = true;
    }

    private void UpdateAnimator()
    {
        stickman.GetComponent<AnimationManager>().SetHSpeed(current_speed);
    }

    public override void Execute(MouseFollower target)
    {
        base.Execute(target);
        
        SetInitialSpeed();
        Debug.Log("initial speed: " + current_speed);
        stickman.GetComponent<AnimationManager>().SwitchState(AnimState.Run);
    }

    private void SetInitialSpeed()
    {
        if (stickman.GetComponent<AnimationManager>().aState == AnimState.Run)
        {
            if (goal_position.x > stickman.transform.position.x)
            {
                if (!stickman.GetComponent<AnimationManager>().GetLookingRight())
                {
                    current_speed = -stickman.GetComponent<AnimationManager>().GetHSpeed();
                }
                else
                {
                    current_speed = 0f;
                }
            }
            else
            {
                if (stickman.GetComponent<AnimationManager>().GetLookingRight())
                {
                    current_speed = -stickman.GetComponent<AnimationManager>().GetHSpeed();
                }
                else
                {
                    current_speed = 0f;
                }
            }
        }
        else
        {
            current_speed = 0f;
        }
    }

    private void StopRunning( bool goingRight)
    {
        stickman.transform.position = goal_position;
        animator.speed = 0;
        ShadeMoveManager.instance.RemoveMove(this);
        if (!isPhantom)
        {
            if(aimerIndex == 0)
            {
                stickman.GetComponent<AnimationManager>().SwitchState(AnimState.Iddle);
                stickman.GetComponent<AnimationManager>().SetHSpeed(0);
            }
            stickman.GetComponent<AnimationManager>().SetLookingRight(goingRight);
            GameManager.instance.InitChoosingMode();
        }
        else
        {
            Destroy(stickman);
        }
    }
}
