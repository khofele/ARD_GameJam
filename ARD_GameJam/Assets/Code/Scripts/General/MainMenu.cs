using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button ngButton;
    public void NewGameIsPressed()
    {
        GameManager.Instance?.StartNewGame();
    }
    public void QuitGameIsPressed()
    {
        GameManager.Instance?.QuitGame();
    }
    //private void Start()
    //{
    //    ngButton.Select();
    //}
}
