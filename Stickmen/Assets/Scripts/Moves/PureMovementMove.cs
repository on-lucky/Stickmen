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
        if (starting_position.x < goal_position.x)
        {
            stickman.GetComponentInChildren<OrientationManager>().LookRight();
        }
        else
        {
            stickman.GetComponentInChildren<OrientationManager>().LookLeft();
        }
    }

    public override void Execute(MouseFollower target)
    {
        goal_position = target.transform.position;
        starting_position = stickman.transform.position;
        TimeBarSlider.instance.StartCounting();
        //ShadeMoveManager.instance.SetUpdating(true);
        ShadeMoveManager.instance.AddMove(this);

        animator.speed = 1;
        if (starting_position.x < goal_position.x)
        {
            stickman.GetComponentInChildren<OrientationManager>().LookRight();
        }
        else
        {
            stickman.GetComponentInChildren<OrientationManager>().LookLeft();
        }

        //stickman.GetComponent<StickmanRunner>().RunTo(target.transform.position, GameManager.instance.local_shade.gameObject);
    }

    public override void SwitchAimer()
    {
        aimerIndex = (aimerIndex + 1) % targets.Length;
        SetTarget(aimerIndex, stickman);
    }

    protected virtual void SetTarget(int index, GameObject sMan)
    {
        if(target != null)
        {
            Destroy(target);
        }

        target = Instantiate(targets[aimerIndex], GameManager.instance.local_stickman.transform);
        target.GetComponentInChildren<LightShafts>().m_Cameras[0] = Camera.main;
        target.GetComponent<MouseFollower>().stickman = sMan;
    }
}
