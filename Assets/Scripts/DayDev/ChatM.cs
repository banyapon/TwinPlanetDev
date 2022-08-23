using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChatM : MonoBehaviour
{
    public static ChatM instance;
    [SerializeField]private GameObject preChat;
    [SerializeField]private TMP_InputField _inputfield;
    [SerializeField]private RectTransform _pointSpawnMessage;
    [SerializeField]private Scrollbar _scrolBar;
    string message;
    [SerializeField]private int limitDisplayMessages = 20;
    [SerializeField]private GameObject allMessages;

    public static Action<string> OnSentMessage = delegate { };

    void Start(){
        if(instance == null){
            instance = this;
        }
    }
    public void SentMessage(){
        message = _inputfield.text;
        if(message == ""){ print("message is null"); return; }
        OnSentMessage.Invoke(message);

        Reset();
        ResetScrolbar();
    }

    public void Created_NewMessage(string fromName,string newMessage){
        GameObject newPreMessage = Instantiate(preChat,_pointSpawnMessage);
        newPreMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fromName+" : ";
        newPreMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newMessage;

        if(allMessages == null) return;
        if(allMessages.transform.childCount > limitDisplayMessages){
            Destroy(allMessages.transform.GetChild(0).gameObject);
        }
    }

    void Reset(){ message = ""; _inputfield.text = "";}
    public void ResetScrolbar(){ _scrolBar.value = 0; }
}
