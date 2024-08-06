using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject inventario;
    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowInventario()
    {
        inventario.SetActive(true);
    }

    public void DisaspperInventario()
    {
        inventario.SetActive(false);
    }
}
