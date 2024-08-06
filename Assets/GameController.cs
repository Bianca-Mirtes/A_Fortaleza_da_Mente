using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void NextScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowInventario(GameObject inventario)
    {
        inventario.SetActive(true);
    }

    public void DisaspperInventario(GameObject inventario)
    {
        inventario.SetActive(false);
    }
}
