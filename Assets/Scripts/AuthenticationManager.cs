using TMPro;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;
    private TMP_InputField email;
    private TMP_InputField password;
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
        email = GameObject.FindGameObjectWithTag("InputEmail").GetComponent<TMP_InputField>();
        password = GameObject.FindGameObjectWithTag("InputPassword").GetComponent<TMP_InputField>();
        FindObjectOfType<NetworkController>().SendRegister(email.textComponent.text, password.textComponent.text);
    }

    // to make the user's login
    public void Login()
    {
        email = GameObject.FindGameObjectWithTag("InputEmail").GetComponent<TMP_InputField>();
        password = GameObject.FindGameObjectWithTag("InputPassword").GetComponent<TMP_InputField>();
        FindObjectOfType<NetworkController>().SendLogin(email.textComponent.text, password.textComponent.text);
    }
}
