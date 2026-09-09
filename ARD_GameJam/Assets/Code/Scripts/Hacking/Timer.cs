using UnityEngine;

public class Timer : MonoBehaviour
{
    private const float m_timerDuration = 120.0f; // TODO Balance value, was 20.0f
    private float m_timerValue = 0.0f;
    private bool m_isTimerRunning = false;

    public static Timer Instance
    {
        get; private set;
    }

    public float TimerValue
    {
        get { return m_timerValue; }
    }

    public void StartTimer()
    {
        m_timerValue = m_timerDuration;
        m_isTimerRunning = true;
    }

    private void RunTimer()
    {
        if(m_isTimerRunning == true && GameManager.Instance.CurrentGameState != GameStates.HACKING)
        {
            m_timerValue -= Time.deltaTime;

            if (m_timerValue <= 0.0f)
            {
                m_isTimerRunning = false;
                GameManager.Instance.SetGameState(GameStates.GAMEOVER);
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
        //DontDestroyOnLoad(gameObject);//sicherheitshalber entfernt, da sonst endlose reload loops
    }

    private void Update()
    {
        RunTimer();
    }
}
