using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button register;
    public Button login;
    public TMP_InputField email;
    public TMP_InputField password;


    private void Awake()
    {
        register.onClick.AddListener(RegisterButtonClick);
        login.onClick.AddListener(LoginButtonClick);
    }

    private async void LoginButtonClick()
    {
        var errorText = await AuthenticationManager.Instance.LoginWithUsernamePasswordAsync(email.textComponent.text, password.textComponent.text);
        Debug.Log(errorText);
    }

    private async void RegisterButtonClick()
    {
        var errorText = await AuthenticationManager.Instance.RegisterWithUsernamePasswordAsync(email.textComponent.text, password.textComponent.text);
        Debug.Log(errorText);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Login()
    {

    }

    public void Register()
    {

    }
}
