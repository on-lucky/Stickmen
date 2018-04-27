using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour {

    public StickmanProfile _profile;

    protected void OnMouseDown()
    {
        if (GameManager.instance.game_state == GameManager.GameState.Choosing)
        {
            ToggleMenu();
        }
    }

    public void SetProfile(StickmanProfile profile)
    {
        _profile = profile;
    }

    public void Init()
    {
        List<Move> moveList = IconOrganiser.MakeMoveList(_profile.powers);
        _profile.SetMoveList(moveList);
        IconOrganiser.OrganiseIcons(moveList);

        GameObject menuCam = GameObject.Find("MenuCamera");
        GetComponentInChildren<Billboard>().subject = menuCam;

        GetComponentInChildren<IconSpawner>().SpawnIcons(moveList);

        GetComponent<Rigidbody>().useGravity = true;
        GameManager.instance.local_shade = this;
    }



    private void ToggleMenu()
    {
        GetComponentInChildren<IconSpawner>().ToggleIcons();
    }
}
