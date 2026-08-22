using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private UIStates m_currentUIState = UIStates.NONE;
    public UIStates currentUIState { get; }//{ return m_currentUIState; }

    public GameObject boxRobotOverlay;
    public GameObject hammerRobotOverlay;
    public GameObject serviceRobotOverlay;

    public TextMeshProUGUI m_txtTimer = null;

    [Header("Hacking UI References")]
    public GameObject m_hackingOverlay = null;
    public TextMeshProUGUI m_txtHackingBinding = null;
    public TextMeshProUGUI m_txtHackingTimer = null;

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

    private void Start()
    {
        SetUIState(UIStates.NONE);
    }

    public void SetUIState(UIStates newUIState)
    {
        switch (newUIState)
        {
            case UIStates.NONE:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = false;
                break;
            case UIStates.BOXROBOT:
                boxRobotOverlay.SetActive(true);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                break;
            case UIStates.HAMMERROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(true);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                break;
            case UIStates.SERVICEROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(true);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                break;
            case UIStates.HACKING:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(true);
                m_txtTimer.enabled = false;
                break;
        }
        m_currentUIState = newUIState;

        Debug.Log("UI State changed to " + m_currentUIState);
    }

    public void ChooseUIStateBasedOnCharState()
    {
        switch(CharController.Instance.CurrentCharState)
        {
            case CharStates.BOX_ROBOT:
                SetUIState(UIStates.BOXROBOT);
                break;
            
            case CharStates.HAMMER_ROBOT:
                SetUIState(UIStates.HAMMERROBOT);
                break;
            
            case CharStates.SERVICE_ROBOT:
                SetUIState(UIStates.SERVICEROBOT);
                break;
            
            default:
                SetUIState(UIStates.NONE);
                break;
        }
    }

    private void ShowHackingInputs()
    {
        if(HackingManager.Instance.IsHacking == false)
        {
            return;
        }

        m_txtHackingBinding.text = "Press " + HackingManager.Instance.RequiredBinding.BindingInputActionReference.action.name;
        m_txtHackingTimer.text = HackingManager.Instance.HackingTimer.ToString("F3");
    }

    private void Update()
    {
        if(m_currentUIState == UIStates.HACKING)
        {
            ShowHackingInputs();
        }

        if(GameManager.Instance.CurrentGameState == GameStates.GAMEOVER)
        {
            SetUIState(UIStates.NONE);
        }

        if(m_currentUIState != UIStates.HACKING && m_currentUIState != UIStates.NONE)
        {
            m_txtTimer.text = Timer.Instance.TimerValue.ToString("F3");
        }
    }
}
