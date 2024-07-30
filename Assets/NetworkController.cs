using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Linq;

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
    public string serverAddress = "";

    // WebSocket for communication with the server
    private WebSocket ws;

    private int playerNumber;
    private List<int> playersInLobby;
    private List<int> playersInGame;

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
            actor = playerNumber + "",
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
                case "Wellcome":
                    // Case "Wellcome", the server sended the player's id that would be used to identify the player inside the game
                    playersInLobby.Add(Int32.Parse(response.payload["playerNumber"]));
                    if(playersInLobby.Count == 2)
                    {
                        playersInGame.Add(playersInLobby[0]);
                        playersInLobby.RemoveAt(0);

                        playersInGame.Add(playersInLobby[0]);
                        playersInGame.RemoveAt(0);
                    }
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
