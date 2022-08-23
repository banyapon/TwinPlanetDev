// Network Manager V1.41 By VF
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace AD.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager main;
        public Vector3 spawnPoint = Vector3.zero;

        private string map;
        private int countRoom;

        public static Action<bool> OnPlayerIsMaster = delegate { };

        private void Awake() 
        {
            Application.runInBackground = true;
            if (main == null) 
            { 
                main = this; DontDestroyOnLoad(gameObject); 
            } 
            else Destroy(gameObject); 
        }
        private void Start() => PhotonNetwork.ConnectUsingSettings();

        public void Join(string map) => StartCoroutine(JoinEnumerator(map));
        public void Join(string map, int identity, string name, byte maxPlayer) => StartCoroutine(JoinEnumerator(map, identity, name, maxPlayer));
        public void CheckMaster()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                OnPlayerIsMaster.Invoke(true);
            }
            else
            {
                OnPlayerIsMaster.Invoke(false);
            }
        }
        IEnumerator JoinEnumerator(string map, int identity = 0, string name = "",byte maxplayer = 100)
        {
            SceneManager.LoadScene("Loading");
            this.map = map;
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            }
            else PhotonNetwork.ConnectUsingSettings();
            yield return new WaitUntil(() => PhotonNetwork.NetworkingClient.Server == ServerConnection.MasterServer && PhotonNetwork.IsConnectedAndReady);
            if (identity == 0) JoinOrCreateRoom(map, maxplayer);
            else JoinOrCreatePrivateRoom(map, name);
        }
        public override void OnJoinedRoom()
        {
            countRoom = 0;
            PhotonNetwork.LoadLevel(map);
        }
        private void JoinOrCreateRoom(string map,byte maxPlayer)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayer;
            PhotonNetwork.JoinOrCreateRoom("N" + map + countRoom, roomOptions, null);
        }
        private void JoinOrCreatePrivateRoom(string map, string name)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom("P" + map + name, roomOptions, null);
            print("P" + map + name);
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if (returnCode == 32765)
            {
                countRoom++;
                Join(map);
            }
        }
        public override void OnDisconnected(DisconnectCause cause) => Join(map);
        public void Clicked_Leaveroom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.DestroyAll();
            }
        }

    }
}
