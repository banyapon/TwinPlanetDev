using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInvite : MonoBehaviour
{
    public Text txtName;
    public GameObject declinePanel;
    public static Action<string, string> OnAccept = delegate { };
    public static Action<string, string> OnDecline = delegate { };
    private string _senderName;
    private void Awake()
    {
        gameObject.SetActive(false);
        CheckInviteBox.OnUIInvite += HandleSenderName;
        PhotonChatController.OnDeclined += HandleOnDeclined;
    }
    private void OnDestroy()
    {
        CheckInviteBox.OnUIInvite -= HandleSenderName;
        PhotonChatController.OnDeclined -= HandleOnDeclined;
    }

    private void HandleOnDeclined()
    {
        gameObject.SetActive(true);
        declinePanel.SetActive(true);
    }

    private void HandleSenderName(string senderName)
    {
        txtName.text = senderName + " ได้ชวนคุณถ่ายรูป";
        gameObject.SetActive(true);
        _senderName = senderName;
    }
    public void Clicked_Accept()
    {
        OnAccept.Invoke(_senderName, "Accept");
    }
    public void Clicked_Decline()
    {
        OnDecline.Invoke(_senderName, "Decline");
    }
}
