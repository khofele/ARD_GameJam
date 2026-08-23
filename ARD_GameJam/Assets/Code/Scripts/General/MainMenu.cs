using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGameIsPressed()
    {
        GameManager.Instance?.StartNewGame();
    }
    public void QuitGameIsPressed()
    {
        GameManager.Instance?.QuitGame();
    }
}
