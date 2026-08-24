using System.Collections;
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
        case GameStates.GAMEOVER:
                StartCoroutine(GameOver());
                break;
        default: 
                Time.timeScale = 1f; Cursor.visible = false; break;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        ReloadLevel();
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

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
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
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Confined;
    }
}
