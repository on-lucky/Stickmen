using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class NetworkPlayer : MonoBehaviour {

    public StickmanProfile _profile;
    public Shade shadeTemplate;

    private Shade _shade;
    private CapsuleCollider _collider;
    private Move current_move;

    private void Awake()
    {
        //_profile = new StickmanProfile();
        _collider = GetComponent<CapsuleCollider>();
    }

    void Start () {

        if(ScreenOrganiser.instance != null)
        {
            ScreenOrganiser.instance.AddPlayerObject(this.gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(9, 12);
        Physics.IgnoreLayerCollision(10, 12);

        //TODO: Remove this
        PlaceStickman(new Vector3(0, 1, 0));
        InitializeShade();
        RegisterSelf();
    }

    protected void OnMouseDown()
    {
        Debug.Log("yo");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaceStickman(new Vector3(-1, 0, 0));

        InitializeShade();

        RegisterSelf();
    }

    private void PlaceStickman(Vector3 pos)
    {
        transform.position = pos;
        GetComponent<Rigidbody>().useGravity = true;
    }

    private void InitializeShade()
    {
        _shade = Instantiate(shadeTemplate, transform);
        _shade.SetProfile(_profile);
        _shade.Init();
    }

    private void RegisterSelf()
    {
        if (GetComponent<NetworkIdentity>() != null) {
            if (GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GameManager.instance.local_stickman = this;
            }
        }
        else
        {
            //this is a non connected stickman
            GameManager.instance.local_stickman = this;
        }
    }

    public void SetUpMove(string move_name)
    {
        _shade.GetComponentInChildren<IconSpawner>().ShowIcons(false);
        current_move = _shade._profile.FindMove(move_name);

        if(current_move != null)
        {
            current_move.SetUp(_shade.gameObject);
        }
        else
        {
            Debug.LogError("Could not find the move: " + move_name);
        }
    }

    public void ExecuteMove(MouseFollower target)
    {
        if (current_move != null)
        {
            current_move.Execute(target);
        }
        else
        {
            Debug.LogError("Could not find the move to execute!");
        }
    }

    public void SpawnPhantom(MouseFollower target)
    {
        if (current_move != null)
        {
            current_move.PhantomExecute(target);
        }
        else
        {
            Debug.LogError("Could not find the move to phantom execute!");
        }
    }

    public void ShowIcons()
    {
        _shade.GetComponentInChildren<IconSpawner>().ShowIcons(true);
    }
}
