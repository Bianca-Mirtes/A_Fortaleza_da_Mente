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
}
