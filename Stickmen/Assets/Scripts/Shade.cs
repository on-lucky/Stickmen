using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour {

    public StickmanProfile _profile;

    private Move current_move;

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
        List<Move> moveList = IconOrganiser.MakeMoveList(_profile.powers, AnimState.Iddle);
        _profile.SetMoveList(moveList);
        IconOrganiser.OrganiseIcons(moveList);

        GameObject menuCam = GameObject.Find("MenuCamera");
        GetComponentInChildren<Billboard>().subject = menuCam;

        GetComponentInChildren<IconSpawner>().SpawnIcons(moveList);

        GetComponent<Rigidbody>().useGravity = true;
        GameManager.instance.local_shade = this;
        GetComponentInChildren<Animator>().speed = 0;

        //TEMPORARY
        TimeBarSlider.instance.SetDuration(7);
    }



    private void ToggleMenu()
    {
        GetComponentInChildren<IconSpawner>().ToggleIcons();
    }

    public void SetCurrentMove(Move m)
    {
        current_move = m;
    }

    public void SwitchAimer()
    {
        current_move.SwitchAimer();
    }

    public void UpdateMoveList()
    {
        Debug.Log(GetComponentInChildren<AnimationManager>().aState);
        List<Move> moveList = IconOrganiser.MakeMoveList(_profile.powers, GetComponentInChildren<AnimationManager>().aState);
        _profile.SetMoveList(moveList);
        IconOrganiser.OrganiseIcons(moveList);
        GetComponentInChildren<IconSpawner>().SpawnIcons(moveList);
    }
}
