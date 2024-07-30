using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);
    }
    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

        Debug.Log("Unity Services Started: " + UnityServices.State);
    }

    private static void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Acess Token: {AuthenticationService.Instance.AccessToken}");
            Debug.Log($"Player Name: {AuthenticationService.Instance.PlayerName}");
        };
        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out");
        };
    }

    // To make the user's register
    public async Task<string> RegisterWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            Debug.Log(username + " " + password);
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        FindObjectOfType<NetworkController>().SendRegister(username, password);
        return "";
    }

    // to make the user's login
    public async Task<string> LoginWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        SceneManager.LoadScene("Lobby");
        return "";
    }
}
