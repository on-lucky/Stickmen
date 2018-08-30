using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class NetworkMessenger : NetworkBehaviour{

    public static NetworkMessenger instance;
    NetworkClient m_client;
    public static short indexListmsg = MsgType.Highest + 1;
    public static short startmsg = MsgType.Ready;
    public int _readyNumber = 0;

    public class IndexListMessage : MessageBase
    {
        public int[] P1indexes;
        public int[] P2indexes;
    }

    private void Awake()
    {
        if (NetworkMessenger.instance != null)
        {
            Debug.LogError("More than one NetworkMessenger in the scene!");
        }
        NetworkMessenger.instance = this;
    }

    public void Init () {
        m_client = NetworkManager.singleton.client;
        m_client.RegisterHandler(indexListmsg, OnServerIndexListMessage);
        m_client.RegisterHandler(startmsg, OnServerStartMessage);
        if (Network.isServer)
        {
            Debug.Log("i am the server");
            NetworkServer.RegisterHandler(indexListmsg, OnServerIndexListMessage);
        }
    }

    public void SendPowerListMessage(int[] indexes1, int[] indexes2)
    {
        IndexListMessage msg = new IndexListMessage();
        msg.P1indexes = indexes1;
        msg.P2indexes = indexes2;
        NetworkServer.SendToAll(indexListmsg, msg);
    }

    private void OnServerIndexListMessage(NetworkMessage netMsg)
    {
        IndexListMessage message = netMsg.ReadMessage<IndexListMessage>();
        List<Power> Player1Powers;
        List<Power> Player2Powers;
        if (isServer)
        {
            Player1Powers = PowerManager.instance.getPowerList(message.P1indexes);
            Player2Powers = PowerManager.instance.getPowerList(message.P2indexes);
        }
        else
        {
            Player1Powers = PowerManager.instance.getPowerList(message.P2indexes);
            Player2Powers = PowerManager.instance.getPowerList(message.P1indexes);
        }
        RandomIconsManager.instance.SpawnRandomIcons(Player1Powers, Player2Powers);
    }

    public void SendReadyToStartMessage()
    {
        var msg = new EmptyMessage();
        NetworkServer.SendToAll(startmsg, msg);
    }

    private void OnServerStartMessage(NetworkMessage netMsg)
    {
        EmptyMessage msg = netMsg.ReadMessage<EmptyMessage>();
        _readyNumber++;
        if(_readyNumber >= ConnexionManager.instance.playersCount)
        {
            SceneManager.LoadScene("Fight Arena Template");
        }
    }

}
