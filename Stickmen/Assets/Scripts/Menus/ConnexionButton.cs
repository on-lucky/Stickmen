using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnexionButton : ButtonSelector {

    public bool isHost = false;
    public string ip = "192.168.1.241";
    public int port = 5000;

    protected override void OnMouseDown()
    {
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.networkPort = port;

        base.OnMouseDown();
        if (isHost)
        {
            NetworkManager.singleton.StartHost();
        }
        else if(!isHost)
        {
            NetworkManager.singleton.StartClient();
        }
        StartCoroutine(StartWaitForServer());
    }

    private IEnumerator StartWaitForServer()
    {
        yield return StartCoroutine(WaitForServer());

        ConnexionManager.instance.Init(NetworkManager.singleton.client);
        NetworkMessenger.instance.Init();

        Destroy(this.transform.parent.gameObject);
    }

    private IEnumerator WaitForServer()
    {
        yield return new WaitForSeconds(1f);
    }

}
