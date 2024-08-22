using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    private TMP_InputField inputText;
    private TMP_Text boxText;

    public static event Action<string> OnMessage;
   
    // Start is called before the first frame update
    void Start()
    {
        inputText = transform.GetChild(2).GetComponent<TMP_InputField>();
        boxText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        OnMessage += AddNewMessage;
    }
    private void OnDestroy()
    {
        OnMessage -= AddNewMessage;
    }

    public void AddNewMessage (string message)
    {
        boxText.text += message;
    }

    public void Send(TMP_InputField message)
    {
        CmdSendMessage(message.textComponent.text);
        inputText.text = string.Empty;
    }

    public void CmdSendMessage(string message)
    {
        NetworkController.Instance.SendChatMessage($"[{NetworkController.Instance.player.username}]: {message}");
    }

    public void EventSendMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
