using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public NetworkPlayer local_stickman;
    public Shade local_shade;
    public GameObject phantom_template;
    public GameObject ground_target;
    public MouseFollower target;

    public enum GameState { Aiming, Executing, Choosing };
    public GameState game_state = GameState.Choosing;

    private MouseFollower current_target;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Debug.LogError("More than one GameManager in the scene!");
        }
        GameManager.instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && game_state == GameState.Aiming)
        {
            InitChoosingMode();
        }
        else if (Input.GetMouseButtonDown(0) && game_state == GameState.Aiming)
        {
            game_state = GameState.Executing;
            local_stickman.ExecuteMove(target);
        }
	}

    public void SpawnTarget()
    {
        //current_target = Instantiate(GameManager.instance.ground_target, GameManager.instance.local_stickman.transform);
    }

    public void InitChoosingMode()
    {
        TimeBarSlider.instance.StopCounting();
        game_state = GameState.Choosing;
        local_stickman.ShowIcons();
    }
}
