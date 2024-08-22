using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public GameObject roomCanvas;
    public GameObject chatCanvas;
    public GameObject roomSystemCanvas;
    private void Update()
    {
        if (NetworkController.Instance.isCreator)
        {
           roomCanvas.transform.GetChild(7).gameObject.SetActive(true);
        }
        else
        {
           roomCanvas.transform.GetChild(7).gameObject.SetActive(false);
        }
    }
    public void CreateRoom(TMP_InputField roomName)
    {
        // Verifica se � uma string vazia
        if(roomName.textComponent.text == string.Empty)
        {
            return;
        }

        NetworkController.Instance.CreateRoom(roomName.textComponent.text);

        roomCanvas.transform.GetChild(4).GetComponent<TMP_Text>().text = NetworkController.Instance.player.username;
        chatCanvas.SetActive(false);
        roomCanvas.SetActive(true);
        roomSystemCanvas.SetActive(false);
    }

    public void JoinRoom(TMP_InputField roomName)
    {
        // verifica se � uma string vazia
        if (roomName.textComponent.text == string.Empty)
        {
            return;
        }
        NetworkController.Instance.JoinRoom(roomName.textComponent.text);

        roomCanvas.transform.GetChild(5).GetComponent<TMP_Text>().text = NetworkController.Instance.player.username;
        chatCanvas.SetActive(false);
        roomCanvas.SetActive(true);
        roomSystemCanvas.SetActive(false);
    }

    public void FeedbackRoom(string feedback, string type)
    {
        if(type == "C")
        {
            TMP_Text feedbackRoom = roomSystemCanvas.transform.GetChild(2).GetComponent<TMP_Text>();
            feedbackRoom.text = feedback;
        }
        else
        {
            TMP_Text feedbackRoom = roomSystemCanvas.transform.GetChild(2).GetComponent<TMP_Text>();
            feedbackRoom.text = feedback;
        }
    }

    public void ExitRoom()
    {
        //NetworkController.Instance.JoinRoom(roomName.textComponent.text);
    }
}
