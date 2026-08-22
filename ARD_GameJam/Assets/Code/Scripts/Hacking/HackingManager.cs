using UnityEngine;

public class HackingManager : MonoBehaviour
{
    private Enemy m_currentHackableEnemy = null;

    public static HackingManager Instance
    {
        get; private set;
    }

    public void SetCurrentHackableEnemy(Enemy _currentEnemy)
    {
        m_currentHackableEnemy = _currentEnemy;
        Debug.Log("Enemy set");
    }

    public void ExecuteHacking()
    {
        if(m_currentHackableEnemy != null && GameManager.Instance.CurrentGameState != GameStates.HACKING)
        {
            if(m_currentHackableEnemy.IsLidOpenable == true)
            {
                GameManager.Instance.SetGameState(GameStates.HACKING); // TODO Start Hacking Minigame -> change Game State after successful hack
                Debug.Log("Hacking started");
                CharController.Instance.SetCharState(m_currentHackableEnemy.CorrespondingCharState);
            }
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
}
