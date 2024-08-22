using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void NextScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void UpdateId(string id, string name)
    {
        GameObject.Find(name).GetComponent<PlayerController>().UpdateID(id);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
