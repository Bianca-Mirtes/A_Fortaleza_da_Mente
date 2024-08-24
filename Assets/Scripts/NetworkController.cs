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
    private ConcurrentQueue<string> feedback = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> fluxo = new ConcurrentQueue<string>();

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

    // WebSocket for communication with the server
    private WebSocket ws;

    //protected string playerID { get; set; }
    public Player player = new Player();

    private string roomNameWs = string.Empty;
    private string updateLabelRoom = string.Empty;
    /*
        private TMP_Text textBox;
        private TMP_Text feedbackC;
        private TMP_Text feedbackJ;*/

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
            while (fluxo.TryDequeue(out string message))
            {
                if(message == "Alguém saiu da Sala")
                {
                    if (isCreator)
                    {
                        SendUpdateRoom(true);
                        FindObjectOfType<LobbyController>().ChangeLabel(5, "Esperando player 2...");
                    }
                    else
                    {
                        isCreator = true;
                        FindObjectOfType<LobbyController>().ChangeLabel(4, player.username);
                        FindObjectOfType<LobbyController>().ChangeLabel(5, "Esperando player 2...");
                        SendUpdateRoom(true);
                    }
                }
                if(message == "Alguém entrou na Sala")
                {
                    FindObjectOfType<LobbyController>().ChangeLabel(5, updateLabelRoom);
                    SendUpdateRoom(false);
                }
                if(message == "Usuario saiu da Sala")
                {
                    if (isCreator)
                    {
                        FindObjectOfType<LobbyController>().ChangeLabel(4, updateLabelRoom);
                        FindObjectOfType<LobbyController>().ChangeLabel(5, "Esperando player 2...");
                        SendUpdateRoom(true);
                    }
                    else
                    {
                        FindObjectOfType<LobbyController>().ChangeLabel(5, "Esperando player 2...");
                        SendUpdateRoom(true);
                    }
                }
            }
            TMP_Text textBox = GameObject.Find("TextBoxChat").GetComponent<TextMeshProUGUI>();
            TMP_Text feedbackC = GameObject.Find("FeedbackCreate").GetComponent<TextMeshProUGUI>();
            TMP_Text feedbackJ = GameObject.Find("FeedbackJoin").GetComponent<TextMeshProUGUI>();
            while (messageChat.TryDequeue(out string message))
            {
                textBox.text += $"\n{message}";
            }
            while (feedbackCreate.TryDequeue(out string message))
            {
                feedbackC.text = message;
                if(message == "Sala criada com sucesso!")
                {
                    feedbackJ.text = string.Empty;
                    feedbackC.text = string.Empty;
                    textBox.text = string.Empty;
                    FindObjectOfType<LobbyController>().ChangeStateCanvas(4);
                    FindObjectOfType<LobbyController>().ChangeLabel(5, "Esperando player 2...");
                }
            }
            while (feedbackJoin.TryDequeue(out string message))
            {
                feedbackJ.text = message;
                if (message == "Sala disponivel!")
                {
                    feedbackC.text = string.Empty;
                    feedbackJ.text = string.Empty;
                    textBox.text = string.Empty;
                    FindObjectOfType<LobbyController>().ChangeStateCanvas(5);
                    FindObjectOfType<LobbyController>().ChangeLabel(4, updateLabelRoom);
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            TMP_Text feedbackMenu = GameObject.FindGameObjectWithTag("Feedback").GetComponent<TextMeshProUGUI>();
            while (feedback.TryDequeue(out string message))
            {
                feedbackMenu.text = message;
                if (message == "Login realizado com sucesso!")
                {
                    SceneManager.LoadScene(1);
                }

            }
        }
        if(SceneManager.GetActiveScene().name == "LeveOne")
        {
            if (GameController.Instance.isStart)
            {
                GameController.Instance.SetisStart(false);
                if (isCreator)
                {
                    CreateElizabeth();
                }
                else
                {
                    CreateAnthony();
                }
            }
        }
    }

   public void SendUpdateRoom(bool state)
    {
        isCreator = true;
        Action action = new Action
        {
            type = "UpdateRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"state", "true"},
                {"roomName", roomNameWs}
            }
        };
        ws.Send(action.ToJson());
    }

    private void ServerConnection()
    {
        ws = new WebSocket(serverAddress);

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
                    feedback.Enqueue("Usuário já possui registro com esse email!");
                    break;
                case "UserAlreadyExistWithThisUsername":
                    feedback.Enqueue("Usuário já possui registro com esse username!");
                    break;
                case "RegisterSucessful":
                    feedback.Enqueue("Usuário registrado com sucesso!");
                    break;
                case "LoginSucessful":
                    feedback.Enqueue("Login realizado com sucesso!");
                    break;
                case "LoginFail_PasswordIncorrect":
                    feedback.Enqueue("Senha Incorreta!");
                    break;
                case "LoginFail_EmailIncorrect":
                    feedback.Enqueue("Email Incorreto!");
                    break;
                case "LoginFail_UsernameIncorrect":
                    feedback.Enqueue("Username Incorreto!");
                    break;
                case "LoginFail_UserNotRegistered":
                    feedback.Enqueue("Usuário não possui registro!");
                    break;
                case "RoomDontExist":
                    feedbackJoin.Enqueue("Não existe uma sala com esse nome!");
                    break;
                case "RoomCreated":
                    feedbackCreate.Enqueue("Sala criada com sucesso!");
                    roomNameWs = response.parameters["roomName"];
                    break;
                case "RoomComplete":
                    feedbackJoin.Enqueue("A sala está cheia! Partida em andamento!");
                    break;
                case "RoomAlreadyExist":
                    feedbackCreate.Enqueue("Já existe uma sala com esse nome!");
                    break;
                case "JoinedInRoom":
                    feedbackJoin.Enqueue("Sala disponivel!");
                    updateLabelRoom = response.parameters["creator"];
                    break;
                case "JoinedSomethingInRoom":
                    fluxo.Enqueue("Alguém entrou na Sala");
                    updateLabelRoom = response.parameters["player2"];
                    break;
                case "ExitedOfTheRoom":
                    fluxo.Enqueue("Usuario saiu da Sala");
                    updateLabelRoom = response.parameters["playerName"];
                    break;
                case "ExitedSomethingOfTheRoom":
                    fluxo.Enqueue("Alguém saiu da Sala");
                    updateLabelRoom = response.parameters["playerName"];
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
                {"roomName", roomName.Trim()}
            }
        };
        ws.Send(action.ToJson());
    }
    public void JoinRoom(string roomName)
    {
        isCreator = false;
        Action action = new Action
        {
            type = "JoinRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"roomName", roomName.Trim()}
            }
        };
        ws.Send(action.ToJson());
    }

    public void ExitRoom()
    {
        Action action = new Action
        {
            type = "ExitRoom",
            actor = player.id,
            parameters = new Dictionary<string, string>() {
                {"roomName", roomNameWs.Trim()}
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
                {"message", message.Trim()}
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
                {"playerEmail", email.Trim()},
                {"playerPassword", password.Trim()},
                {"playerUsername", username.Trim()}
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
