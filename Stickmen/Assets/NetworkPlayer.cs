using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {

    public StickmanProfile _profile;

    private void Awake()
    {
        _profile = new StickmanProfile();
    }

    void Start () {
        ScreenOrganiser.instance.AddPlayerObject(this.gameObject);
	}
}
