using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class VNetwork : MonoBehaviourPunCallbacks
{
    public const string MAP_KEY = "map";
    //string gameVersion = "1";

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 500, 100), 
            "Connection Status: " + PhotonNetwork.NetworkClientState +
            "\nIs Master Client: " + PhotonNetwork.IsMasterClient
        );
        if (GUI.Button(new Rect(10, Screen.height - 60, 100, 50), "Connect"))
        {
            Connect();
        }
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        //Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            print("A");
            QuickMatch();
            //JoinOrCreatePrivateRoom("dog");
            //PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            print("B");
            PhotonNetwork.NickName = "N"+Random.Range(1000, 9999);
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void QuickMatch(byte map = 0)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 100;
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
        //Hashtable hashtable = new Hashtable { { MAP_KEY, 3 } };
        //roomOptions.CustomRoomProperties = hashtable;
        //PhotonNetwork.JoinRandomOrCreateRoom(hashtable, roomOptions: roomOptions);
    }
    public void JoinOrCreatePrivateRoom(string nameEveryFriendKnows)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 100;
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom(nameEveryFriendKnows, roomOptions, null);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("L: OnConnectedToMaster");
    }

    public override void OnCreatedRoom()
    {
        print("L: OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        print("L: OnJoinedRoom *****");
        /*
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object data;
            if (player.CustomProperties.TryGetValue("Key", out data))
            {
                bool b = (bool)data;
            }
        }
        */

        Vector3 position = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("Cube", position, Quaternion.identity);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("L: OnJoinRandomFailed");
        //CreateRoom();
    }

    public override void OnLeftRoom()
    {
        print("L: OnLeftRoom");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("L: OnPlayerEnteredRoom");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("L: OnPlayerLeftRoom");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        print("L: OnMasterClientSwitched");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        print("L: OnPlayerPropertiesUpdate *****");
    }

    /*
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
    }
    */
}

