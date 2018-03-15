using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkPlayer : MonoBehaviour {

    public StickmanProfile _profile;

    private void Awake()
    {
        //_profile = new StickmanProfile();
    }

    void Start () {
        ScreenOrganiser.instance.AddPlayerObject(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        List<Move> moveList = IconOrganiser.MakeMoveList(_profile.powers);
        IconOrganiser.OrganiseIcons(moveList);


        GameObject menuCam = GameObject.Find("MenuCamera");
        GetComponentInChildren<Billboard>().subject = menuCam;

        GetComponentInChildren<IconSpawner>().SpawnIcons(moveList);

        transform.position = new Vector3(-1, 0, 0);
    }
}
