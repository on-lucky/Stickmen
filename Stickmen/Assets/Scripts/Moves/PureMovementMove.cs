using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new movement move", menuName = "movement move")]
public class PureMovementMove : Move {

    protected Vector3 goal_position;
    protected Vector3 starting_position;

    public GameObject[] targets;
    protected GameObject target;

    /*public PureMovementMove(GameObject _stickman, Vector3 startingPos, Vector3 goalPos)
    {
        stickman = _stickman;
        starting_position = startingPos;
        goal_position = goalPos;
        animator = stickman.GetComponentInChildren<Animator>();
    }*/

    public override void SetUp(GameObject _stickman)
    {
        base.SetUp(_stickman);
        GameManager.instance.game_state = GameManager.GameState.Aiming;
        stickman.GetComponent<PhantomSpawner>().StartSpawning();
        SetIsPhantom();

        SetTarget(aimerIndex, _stickman);
    }

    public override void PhantomExecute()
    {
        //Debug.Log("going from: " + starting_position + " to :" + goal_position);
        animator.speed = 1;

        stickman.GetComponentInChildren<OrientationManager>().LookTo(starting_position.x < goal_position.x);
    }

    public override void Execute(MouseFollower target)
    {
        goal_position = target.transform.position;
        starting_position = stickman.transform.position;
        TimeBarSlider.instance.StartCounting();
        //ShadeMoveManager.instance.SetUpdating(true);
        ShadeMoveManager.instance.AddMove(this);

        animator.speed = 1;
        stickman.GetComponentInChildren<OrientationManager>().LookTo(starting_position.x < goal_position.x);

        //stickman.GetComponent<StickmanRunner>().RunTo(target.transform.position, GameManager.instance.local_shade.gameObject);
    }

    public override void SwitchAimer()
    {
        aimerIndex = (aimerIndex + 1) % targets.Length;
        SetTarget(aimerIndex, stickman);
    }

    protected virtual void SetTarget(int index, GameObject sMan)
    {
        if (target != null)
        {
            // Gather info on the last target
            Vector3 pos = target.transform.position;
            Quaternion rot = target.transform.rotation;
            bool lookingRight = target.GetComponent<MouseFollower>().GetLookingRight();
            Vector3 borderPos = target.GetComponent<MouseFollower>().GetLastBorderPos();
            float snapDist = target.GetComponent<MouseFollower>().snap_distance;
            bool onGround = target.GetComponent<MouseFollower>().GetIsOnGround();
            bool switchable = target.GetComponent<MouseFollower>().Switchable;

            Destroy(target);

            //use last target info to spawn new target
            target = Instantiate(targets[aimerIndex], pos, rot);
            target.GetComponent<MouseFollower>().SetLookingRight(lookingRight);
            target.GetComponent<MouseFollower>().SetLastBorderPos(borderPos);
            target.GetComponent<MouseFollower>().snap_distance = snapDist;
            target.GetComponent<MouseFollower>().SetIsOnGround(onGround);
            target.GetComponent<MouseFollower>().Switchable = switchable;
        }
        else
        {
            target = Instantiate(targets[aimerIndex], GameManager.instance.local_stickman.transform);
        }
        
        target.GetComponentInChildren<LightShafts>().m_Cameras[0] = Camera.main;
        target.GetComponent<MouseFollower>().stickman = sMan;
    }
}
