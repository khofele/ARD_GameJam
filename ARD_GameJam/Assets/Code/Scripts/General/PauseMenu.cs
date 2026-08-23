using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public InputActionReference pauseInActRef;
    private void OnEnable()
    {
        pauseInActRef.action.Enable();
    }
    private void OnDisable()
    {
        pauseInActRef.action.Disable();
    }
    private void Resume()
    {
        GameManager.Instance?.SetGameState(GameStates.RUNNING);
    }
    private void Pause()
    {
        if (GameManager.Instance?.CurrentGameState == GameStates.GAMEOVER || GameManager.Instance?.CurrentGameState == GameStates.HACKING) return;
        GameManager.Instance?.SetGameState(GameStates.PAUSED);
    }
    public void ToggleMenu()
    {
        if (GameManager.Instance?.CurrentGameState == GameStates.PAUSED)
            Resume();
        else
            Pause();
    }
    public void GoToMainMenuIsPressed()
    {
        GameManager.Instance?.GoToMainMenu();
    }
    public void ReloadLevelIsPressed()
    {
        GameManager.Instance?.ReloadLevel();
    }
    public void QuitGameIsPressed()
    {
        GameManager.Instance?.QuitGame();
    }
    private void Update()
    {
        if (pauseInActRef.action.WasPerformedThisFrame())
            ToggleMenu();
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) Pause();
    }
}
