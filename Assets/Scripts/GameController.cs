using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameController");
                    instance = obj.AddComponent<GameController>();
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
    public bool isStart = false;
    public void NextScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void UpdateId(string id, string name)
    {
        GameObject.Find(name).GetComponent<PlayerController>().UpdateID(id);
    }

    public bool GetisStart()
    {
        return isStart;
    }

    public void SetisStart(bool value)
    {
        isStart = value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
