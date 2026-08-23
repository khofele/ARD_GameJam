using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public InputActionReference pauseInActRef;
    public GameObject pauseMenu;
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
        pauseMenu.SetActive(false);
    }
    private void Pause()
    {
        if (GameManager.Instance?.CurrentGameState == GameStates.GAMEOVER || GameManager.Instance?.CurrentGameState == GameStates.HACKING) return;
        GameManager.Instance?.SetGameState(GameStates.PAUSED);
        pauseMenu.SetActive(true);
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
        Debug.Log("ReloadLevelPressed");
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
