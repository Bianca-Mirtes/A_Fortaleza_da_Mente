using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance { get; private set; }
    public TMP_InputField email;
    public TMP_InputField password;
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
    // To make the user's register
    public void Register()
    {
        FindObjectOfType<NetworkController>().SendRegister(email.textComponent.text, password.textComponent.text);
    }

    // to make the user's login
    public void Login()
    {
        FindObjectOfType<NetworkController>().SendLogin(email.textComponent.text, password.textComponent.text);
    }
}
