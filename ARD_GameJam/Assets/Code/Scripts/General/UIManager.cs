using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private UIStates m_currentUIState = UIStates.NONE;
    public UIStates currentUIState { get; }//{ return m_currentUIState; }

    public GameObject boxRobotOverlay;
    public GameObject hammerRobotOverlay;
    public GameObject serviceRobotOverlay;

    public static UIManager Instance
    {
        get; private set;
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

    public void SetUIState(UIStates newUIState)
    {
        switch (newUIState)
        {
            case UIStates.NONE:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                break;
            case UIStates.BOXROBOT:
                boxRobotOverlay.SetActive(true);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                break;
            case UIStates.HAMMERROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(true);
                serviceRobotOverlay.SetActive(false);
                break;
            case UIStates.SERVICEROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(true);
                break;
        }
        m_currentUIState = newUIState;
    }
}
