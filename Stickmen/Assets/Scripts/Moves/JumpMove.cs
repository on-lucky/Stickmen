using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new jump move", menuName = "jump move")]
public class JumpMove : PureMovementMove {

    public GameObject arcAimer;
    public float speed_multiplier = 1f;
    public float jumpDelay = 1f;
    public float landDelay = 1f;

    private Vector3[] arc_positions;
    private int arc_pts_count;
    private GameObject aimer_instance;
    private float stickman_speed;
    private float current_pos_x;
    private float jump_distance;
    private float jump_height;
    public float acceleration = 200f;
    private float currentTime = 0f;
    private bool isLanding = false;
    private bool willLand = false;

    public override void SetUp(GameObject _stickman)
    {
        base.SetUp(_stickman);
        target.GetComponent<MouseFollower>().stick_to_platform = false;
        target.GetComponent<MouseFollower>().snap_distance = 2f;
        currentTime = 0f;
        isLanding = false;

        if (arcAimer != null)
        {
            aimer_instance = Instantiate(arcAimer, stickman.transform);
            arc_pts_count = aimer_instance.GetComponent<ArcAimer>().numberOfPoints + 1;
            arc_positions = new Vector3[arc_pts_count];
            aimer_instance.GetComponent<LineRenderer>().GetPositions(arc_positions);
            aimer_instance.GetComponent<ArcAimer>().objToFollow = target;
        }
    }

    public void Init(GameObject _stickman, Vector3 startingPos, Vector3 goalPos, Vector3[] arc_pos, int pts_count)
    {
        stickman = _stickman;
        starting_position = startingPos;
        current_pos_x = 0;
        jump_distance = Mathf.Abs(startingPos.x - goalPos.x);

        Vector3 topPoint = FindTopPoint(arc_pos);
        jump_height = topPoint.y - arc_pos[0].y;
        float jump_mid_x = Mathf.Abs(topPoint.x - arc_pos[0].x);
        stickman_speed = jump_mid_x / FindJumpTime();

        goal_position = goalPos;
        animator = stickman.GetComponentInChildren<Animator>();
        
        arc_positions = arc_pos;
        arc_pts_count = pts_count;
        SetIsPhantom();
        currentTime = 0f;
        isLanding = false;
    }

    public override void SpawnPhantom(MouseFollower target)
    {
        GameObject phantom = Instantiate(GameManager.instance.phantom_template, stickman.transform);

        JumpMove phantomMove = ScriptableObject.CreateInstance<JumpMove>();

        aimer_instance.GetComponent<LineRenderer>().GetPositions(arc_positions);
        phantomMove.Init(phantom, stickman.transform.position, target.transform.position, arc_positions, arc_pts_count);

        ShadeMoveManager.instance.AddMove(phantomMove);
        phantom.GetComponent<AnimationManager>().SwitchState(AnimState.Jump);
        phantomMove.PhantomExecute();
    }

    public override bool UpdateMethod(float deltaTime)
    {

        bool isMoveOver = false;

        if (stickman != null)
        {
            currentTime += deltaTime;
            if (!isLanding)
            {
                if (currentTime > jumpDelay)
                {

                    float mov_distance = deltaTime * stickman_speed;
                    current_pos_x += mov_distance;

                    int first_point_index = (int)((current_pos_x / jump_distance) * (arc_pts_count - 1));
                    if (first_point_index < arc_pts_count - 1)
                    {

                        float ratio_between_pts = (current_pos_x % (jump_distance / (arc_pts_count - 1))) / (jump_distance / (arc_pts_count - 1));

                        Vector3 new_pos = Vector3.Lerp(arc_positions[first_point_index], arc_positions[first_point_index + 1], ratio_between_pts);

                        stickman.transform.position = new_pos;
                    }
                    else
                    {
                        stickman.transform.position = arc_positions[arc_positions.Length - 1];

                        if (willLand)
                        {
                            isLanding = true;
                            currentTime = 0f;
                        }
                        else
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
                }
            }
            else
            {
                if(currentTime > landDelay)
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
        }
        return isMoveOver;
    }

    private Vector3 FindTopPoint(Vector3[] arc_points)
    {
        Vector3 top_point = arc_points[0];

        foreach (Vector3 point in arc_points){
            if(point.y > top_point.y)
            {
                top_point = point;
            }
        }

        return top_point;
    }

    private float FindJumpTime()
    {
        return Mathf.Sqrt((2 * jump_height) / acceleration);
    }

    public override void Execute(MouseFollower target)
    {
        base.Execute(target);

        willLand = target.GetComponent<MouseFollower>().GetIsOnGround();
        Destroy(aimer_instance);
        stickman.GetComponent<Rigidbody>().isKinematic = true;
        aimer_instance.GetComponent<LineRenderer>().GetPositions(arc_positions);
        this.Init(stickman, stickman.transform.position, target.transform.position, arc_positions, arc_pts_count);

        stickman.GetComponent<AnimationManager>().SwitchState(AnimState.Jump);
    }
}
