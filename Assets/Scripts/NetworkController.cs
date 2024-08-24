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
using System.IO.IsolatedStorage;
using System.Collections.Concurrent;

public class NetworkController : MonoBehaviour
{
    private ConcurrentQueue<string> messageChat = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> feedbackCreate = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> feedbackJoin = new ConcurrentQueue<string>();

    // Elizabeth's Prefab
    public GameObject ElizabethPrefab;

    // Elizabeth Initial Position
    public Transform positionP1;

    // Anthony's Prefab
    public GameObject AnthonyPrefab;

    // Anthony Initial Position
    public Transform positionP2;

    // Server Adress
    public string serverAddress = "ws://localhost:9000";

    public TextMeshProUGUI feedback;

    // WebSocket for communication with the server
    private WebSocket ws;

    //protected string playerID { get; set; }
    public Player player = new Player();

    private string roomNameWs = string.Empty;

    private bool isConnected = false;
    public bool isCreator { get; set; }

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
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            TMP_Text textBox = GameObject.Find("TextBox").GetComponent<TextMeshProUGUI>();
            TMP_Text feedbackC = GameObject.Find("FeedbackC").GetComponent<TextMeshProUGUI>();
            TMP_Text feedbackJ = GameObject.Find("FeedbackJ").GetComponent<TextMeshProUGUI>();
            while (messageChat.TryDequeue(out string message))
            {
                textBox.text = message;
            }
            while (feedbackCreate.TryDequeue(out string message))
            {
                feedbackC.text = message;
            }
            while (feedbackJoin.TryDequeue(out string message))
            {
                feedbackJ.text = message;
            }
        }   
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
    public void SendRegister(string email, string password, string username)
    {
        feedback = GameObject.FindGameObjectWithTag("Feedback").GetComponent<TextMeshProUGUI>();
        player.email = email;
        player.password = password;
        player.username = username;
        Action action = new Action
        {
            type = "Register",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"playerEmail", email},
                {"playerPassword", password},
                {"playerUsername", username}
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
                    player.id = response.parameters["playerID"];
                    break;
                case "Chat":
                    messageChat.Enqueue(response.parameters["message"]);
                    break;
                case "UserAlreadyExistWithThisEmail":
                    feedback.text = "Usuário já possui registro com esse email!";
                    break;
                case "UserAlreadyExistWithThisUsername":
                    feedback.text = "Usuário já possui registro com esse username!";
                    break;
                case "RegisterSucessful":
                    feedback.text = "Usuário registrado com sucesso!";
                    break;
                case "LoginSucessful":
                    feedback.text = "Login realizado com sucesso!";
                    Invoke("LoadLobby", 2f);
                    break;
                case "LoginFail_PasswordIncorrect":
                    feedback.text = "Senha Incorreta!";
                    break;
                case "LoginFail_EmailIncorrect":
                    feedback.text = "Email Incorreto!";
                    break;
                case "LoginFail_UsernameIncorrect":
                    feedback.text = "Username Incorreto";
                    break;
                case "LoginFail_UserNotRegistered":
                    feedback.text = "Usuário não possui registro!";
                    break;
                case "RoomDontExist":

                    FindObjectOfType<LobbyController>().FeedbackRoom("N�o existe uma sala com esse nome!", "J");
                    break;
                case "RoomCreated":
                    FindObjectOfType<LobbyController>().FeedbackRoom("Sala criada com sucesso!", "C");
                    roomNameWs = response.parameters["roomName"];
                    FindObjectOfType<LobbyController>().ChangeStateCanvas(4);
                    break;
                case "RoomComplete":
                    FindObjectOfType<LobbyController>().FeedbackRoom("A sala está cheia! Partida em andamento!", "J");
                    break;
                case "RoomAlreadyExist":
                    FindObjectOfType<LobbyController>().FeedbackRoom("Já existe uma sala com esse nome!", "C");
                    break;
                case "JoinedInRoom":
                    FindObjectOfType<LobbyController>().FeedbackRoom("Sala disponível!", "J");
                    FindObjectOfType<LobbyController>().ChangeStateCanvas(5);
                    break;
                case "ExitRoomSucessful":
                    FindObjectOfType<LobbyController>().UpdateLabels(response.parameters["player1Name"], response.parameters["player2Name"]);
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

    private void Start()
    {
        if (!isConnected)
        {
            ServerConnection();
            isConnected = true;
        }
    }

    public void CreateRoom(string roomName)
    {
        isCreator = true;
        Action action = new Action
        {
            type = "CreateRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"roomName", roomName},
                {"creator", player.username}
            }
        };
        ws.Send(action.ToJson());
    }
    public void JoinRoom(string roomName)
    {
        Action action = new Action
        {
            type = "JoinRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"roomName", roomName},
                {"playerName", player.username}
            }
        };
        ws.Send(action.ToJson());
    }
    private void LoadLobby()
    {
        FindObjectOfType<GameController>().NextScene(1);
    }

    public void ExitRoom()
    {
        Action action = new Action
        {
            type = "ExitRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"roomName", roomNameWs},
                {"playerName", player.username}
            }
        };
        ws.Send(action.ToJson());
    }

    public void SendChatMessage(string message)
    {
        Action action = new Action
        {
            type = "Chat",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"message", message}
            }
        };
        ws.Send(action.ToJson());
    }

    public void SendLogin(string email, string password, string username)
    {
        Action action = new Action
        {
            type = "Login",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"playerEmail", email},
                {"playerPassword", password},
                {"playerUsername", username}
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
