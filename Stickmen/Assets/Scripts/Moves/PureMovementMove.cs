using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new movement move", menuName = "movement move")]
public class PureMovementMove : Move {

    private GameObject _stickman;

    public override void SetUp(GameObject stickman)
    {
        _stickman = stickman;
        GameManager.instance.game_state = GameManager.GameState.Aiming;
        _stickman.GetComponent<PhantomSpawner>().StartSpawning();

        GameObject target = Instantiate(GameManager.instance.ground_target, GameManager.instance.local_stickman.transform);
        target.GetComponent<MouseFollower>().stick_to_platform = true;
        target.GetComponentInChildren<LightShafts>().m_Cameras[0] = Camera.main;
    }

    public override void PhantomExecute(MouseFollower target)
    {
        GameObject phantom = Instantiate(GameManager.instance.phantom_template, _stickman.transform);
        Vector3 pos = target.transform.position;
        phantom.GetComponent<StickmanRunner>().RunTo(target.transform.position, phantom);
    }

    public override void Execute(MouseFollower target)
    {
        Vector3 pos = target.transform.position;
        _stickman.GetComponent<StickmanRunner>().RunTo(target.transform.position, GameManager.instance.local_shade.gameObject);
    }
}
