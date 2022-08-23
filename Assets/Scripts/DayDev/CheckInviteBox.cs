using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.UI;

public class CheckInviteBox : MonoBehaviour
{
    public Collider currentTarget;
    private int viewid;

    public static Action<string,string> OnInvitePlayer = delegate { };
    public static Action<string> OnUIInvite = delegate { };

    private void Awake()
    {
        PhotonChatController.OnRoomInvite += HandleRoomInvite;
    }
    private void OnDestroy()
    {
        PhotonChatController.OnRoomInvite -= HandleRoomInvite;
    }

    private void HandleRoomInvite(string sender, string roomName)
    {
        OnUIInvite.Invoke(sender);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChatCheck"))
        {
            gameObject.GetComponent<Collider>().enabled = false;
            currentTarget = other;
            viewid = other.transform.parent.GetComponent<PhotonView>().ViewID;
            Debug.Log(viewid);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == currentTarget)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }
    public void Clicked_Invite()
    {
        OnInvitePlayer.Invoke(viewid.ToString(), $"{PlayerPrefs.GetString("nickname")}'s room");
    }
}
