using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private GameStates m_currentGameState = GameStates.PAUSED;

    public static GameManager Instance
    {
        get; private set;
    }

    public void SetGameState(GameStates _newGameState)
    {
        m_currentGameState = _newGameState;
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
}
