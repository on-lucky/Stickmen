using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class ConnexionManager : NetworkBehaviour {

    public string ip = "192.168.1.241";
    public int playersCount;

    NetworkClient m_client;

    const short readyMsg = 1002;

    public static ConnexionManager instance;                 // Singleton of the MenuManager
    private bool gameStartToggle = false;

    private void Awake()
    {
        if (ConnexionManager.instance != null)
        {
            Debug.LogError("More than one ConnexionManager in the scene!");
        }
        ConnexionManager.instance = this;
    }

    public void Init(NetworkClient client)
    {
        m_client = client;
        m_client.RegisterHandler(readyMsg, OnServerReadyToBeginMessage);
        NetworkServer.RegisterHandler(readyMsg, OnServerReadyToBeginMessage);
    }


    // Update is called once per frame
    void Update () {

        if (NetworkManager.singleton.numPlayers >= playersCount && !gameStartToggle)
        {
            StartCoroutine(StartWaitForServer());
            gameStartToggle = true;
        }
	}

    public void SendReadyToBeginMessage()
    {
        var msg = new EmptyMessage();
        NetworkServer.SendToAll(readyMsg, msg);
    }

    void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
        //SceneManager.LoadScene("RandomPowerSelector");
        //NetworkManager.singleton.ServerChangeScene("RandomPowerSelector");
        RandomPowerGenerator.instance.StartGeneration();
    }

    private IEnumerator StartWaitForServer()
    {
        yield return StartCoroutine(WaitForServer());
        SendReadyToBeginMessage();
    }

    private IEnumerator WaitForServer()
    {
        yield return new WaitForSeconds(1f);
    }

    public bool CheckifServer()
    {
        return isServer;
    }
}
