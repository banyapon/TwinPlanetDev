using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using Photon.Realtime;

public class PhotonChatController : MonoBehaviourPunCallbacks, IChatClientListener
{
    private ChatClient chatClient;
    public static Action<string, string> OnRoomInvite = delegate { };
    public static Action OnDeclined = delegate { };
    private bool master;
    private bool follower;
    private void Start()
    {
        chatClient = new ChatClient(this);
    }
    private void Awake()
    {
        CheckInviteBox.OnInvitePlayer += HandleOnInvite;
        UIInvite.OnAccept += HandleOnAccept;
        UIInvite.OnDecline += HandleOnDecline;
    }
    private void OnDestroy()
    {
        CheckInviteBox.OnInvitePlayer -= HandleOnInvite;
        UIInvite.OnAccept -= HandleOnAccept;
        UIInvite.OnDecline -= HandleOnDecline;
    }

    private void HandleOnDecline(string recipient, string message)
    {
        chatClient.SendPrivateMessage(recipient, message);
    }

    private void HandleOnAccept(string recipient, string message)
    {
        chatClient.SendPrivateMessage(recipient, message);
        PhotonNetwork.LeaveRoom();
        follower = true;
    }
    
    public void HandleOnInvite(string recipient,string roomName)
    {
        chatClient.SendPrivateMessage(recipient, roomName);
    }
    public void DebugReturn(DebugLevel level, string message) { }
    public void OnChatStateChange(ChatState state) { }
    public void OnConnected() { }
    public void OnDisconnected() { }
    public void OnGetMessages(string channelName, string[] senders, object[] messages) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnSubscribed(string[] channels, bool[] results) { }
    public void OnUnsubscribed(string[] channels) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            string[] splitNames = channelName.Split(new char[] { ':' });
            string senderName = splitNames[0];

            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{sender}: {message}");
                if (message.ToString() == "Accept")
                {
                    PlayerPrefs.SetString("PhotoboothRoomName", $"{PlayerPrefs.GetString("nickname")}'s room");
                    PhotonNetwork.LeaveRoom();
                    master = true;
                }
                else if (message.ToString() == "Decline")
                {
                    OnDeclined.Invoke();
                }
                else
                {
                    OnRoomInvite.Invoke(sender, message.ToString());
                    PlayerPrefs.SetString("PhotoboothRoomName", message.ToString());
                }
            }
        }
    }
    public override void OnLeftRoom()
    {
        if (master)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2,
                IsOpen = true,
                IsVisible = true,
                PublishUserId = false,
                CleanupCacheOnLeave = false,
            };
            PhotonNetwork.CreateRoom($"{PlayerPrefs.GetString("nickname")}'s room", roomOptions);
        }
        if (follower && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRoom($"{PlayerPrefs.GetString("nickname")}'s room");
            return;
        }
    }
}
