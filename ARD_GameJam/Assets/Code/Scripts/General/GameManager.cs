using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameStates m_currentGameState = GameStates.RUNNING;

    public GameStates CurrentGameState 
    { 
        get { return m_currentGameState; } 
    }

    public static GameManager Instance
    {
        get; private set;
    }

    public void SetGameState(GameStates _newGameState)
    {
        m_currentGameState = _newGameState;

        Debug.Log("Game State changed to " + _newGameState);

        switch (m_currentGameState) { 
        case GameStates.PAUSED:
                Time.timeScale = 0f; Cursor.visible = true; break;
        case GameStates.HACKING:
                Cursor.visible = true; break;
        default: 
                Time.timeScale = 1f; Cursor.visible = false; break;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene((int)GameScenes.Intro);//Intro
        SetGameState(GameStates.RUNNING);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene((int)GameScenes.MainMenu);//MainMenu
        SetGameState(GameStates.PAUSED);
    }
    public void ReloadLevel()
    {
        Debug.Log("Reloading Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetGameState(GameStates.RUNNING);
    }
}
