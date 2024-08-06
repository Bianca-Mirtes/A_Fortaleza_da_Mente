using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
    // Elizabeth's Prefab
    public GameObject ElizabethPrefab;

    // Elizabeth Initial Position
    public Transform positionP1;

    // Anthony's Prefab
    public GameObject AnthonyPrefab;

    // Anthony Initial Position
    public Transform positionP2;

    // Server Adress
    public string serverAddress = "ws://192.168.0.10:9000";

    public TextMeshProUGUI feedback;

    // WebSocket for communication with the server
    private WebSocket ws;

    private string playerID;
    private List<String> playersInLobby;
    private List<String> playersInGame;

    private static NetworkController instance;

    public static NetworkController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("NetworkController");
                    instance = obj.AddComponent<NetworkController>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        ServerConnection();
    }

    private void ServerConnection()
    {
        ws = new WebSocket(this.serverAddress);

        // OnMessage is called always that a message is received
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);
            // To process the message received
            ProcessMessage(e.Data);
        };

        // After define the events, to connect at server
        ws.Connect();
    }

    public void SendRegister(string email, string password)
    {
        Action action = new Action
        {
            type = "Register",
            actor = playerID,
            parameters = new Dictionary<string, string>() {
                {"playerEmail", email},
                {"playerPassword", password}
            }
        };
        ws.Send(action.ToJson());
    }

    private void ProcessMessage(string message)
    {
        Debug.Log("Processing message from server");
        try
        {
            // to convert the message received (JSON) for a ServerResponse object
            var response = JsonConvert.DeserializeObject<ServerResponse>(message);

            // To verif the type of message received
            switch (response.type)
            {
                case "Welcome":
                    // Case "Wellcome", the server sended the player's id that would be used to identify the player inside the game
                    Debug.Log("Welcome");
                    playerID = response.payload["playerID"];
                    playersInLobby.Add(response.payload["playerID"]);
                    if(playersInLobby.Count == 2)
                    {
                        SceneManager.LoadScene("LevelOne");
                        CreateElizabeth();
                        GameObject.Find("Elizabeth").GetComponent<PlayerController>().UpdateID(playersInLobby[0]);
                        playersInLobby.RemoveAt(0);

                        CreateAnthony();
                        GameObject.Find("Anthony").GetComponent<PlayerController>().UpdateID(playersInLobby[0]);
                        playersInLobby.RemoveAt(0);
                    }
                    break;
                case "UserAlreadyExist":
                    feedback.text = "Usuário já possui registro!";
                    break;

                case "RegisterSucessful":
                    feedback.text = "Usuário registrado com sucesso!";
                    break;

                case "LoginSucessful":
                    feedback.text = "Login realizado com sucesso!";
                    SceneManager.LoadScene("Lobby");
                    break;

                case "LoginFail_PasswordIncorrect":
                    feedback.text = "Senha Incorreta!";
                    break;
                case "LoginFail_EmailIncorrect":
                    feedback.text = "Email Incorreto!";
                    break;
                case "LoginFail_UserNotRegistered":
                    feedback.text = "Usuário não possui registro!";
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Erro ao deserializar: " + e);
        }
    }

    public void SendLogin(string email, string password)
    {
        Action action = new Action
        {
            type = "Login",
            actor = playerID,
            parameters = new Dictionary<string, string>() {
                {"playerEmail", email},
                {"playerPassword", password}
            }
        };
        ws.Send(action.ToJson());
    }
    public void CreateElizabeth()
    {
        Transform parent = GameObject.Find("PlayerOne").transform;
        Instantiate(ElizabethPrefab, positionP1.position, Quaternion.identity, parent);
    }
    public void CreateAnthony()
    {
        Transform parent = GameObject.Find("PlayerTwo").transform;
        Instantiate(AnthonyPrefab, positionP2.position, Quaternion.identity, parent);
    }
}
