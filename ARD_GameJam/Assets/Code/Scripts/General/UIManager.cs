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

    public GameObject deathscreen;

    public TextMeshProUGUI m_txtTimer = null;

    [Header("Hacking UI References")]
    public GameObject m_hackingOverlay = null;
    public TextMeshProUGUI m_txtHackingBinding = null;
    public TextMeshProUGUI m_txtHackingTimer = null;
    public TextMeshProUGUI m_txtOpenLid = null;

    public static UIManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        //hier statt in start, sonst wird nur einmal ausgeführt
        SetUIState(UIStates.NONE);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //private void Start()
    //{
    //    //SetUIState(UIStates.NONE);
    //    //deathscreen.SetActive(false);
    //}

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
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(false);
                break;
            case UIStates.BOXROBOT:
                boxRobotOverlay.SetActive(true);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(false);
                break;
            case UIStates.HAMMERROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(true);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(false);
                break;
            case UIStates.SERVICEROBOT:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(true);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = true;
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(false);
                break;
            case UIStates.HACKING:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(true);
                m_txtTimer.enabled = false;
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(false);
                break;
            case UIStates.DEATHSCREEN:
                boxRobotOverlay.SetActive(false);
                hammerRobotOverlay.SetActive(false);
                serviceRobotOverlay.SetActive(false);
                m_hackingOverlay.SetActive(false);
                m_txtTimer.enabled = false;
                m_txtOpenLid.enabled = false;
                deathscreen.SetActive(true);
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

    public void EnableHackingIndicator()
    {
        m_txtOpenLid.enabled = true;
    }

    public void DisableHackingIndicator()
    {
        m_txtOpenLid.enabled = false;
    }

    private void ShowHackingInputs()
    {
        if(HackingManager.Instance.IsHacking == false)
        {
            return;
        }
        //Error nullref
        m_txtHackingBinding.text = "Press " + HackingManager.Instance.RequiredBinding.BindingInputActionReference.action.name;
        m_txtHackingTimer.text = HackingManager.Instance.HackingTimer.ToString("F3");
    }

    private void Update()
    {
        if(m_currentUIState == UIStates.HACKING)
        {
            ShowHackingInputs();
        }

        if (GameManager.Instance.CurrentGameState == GameStates.GAMEOVER)
        {
            //SetUIState(UIStates.NONE);
            //deathscreen.SetActive(true);

            SetUIState(UIStates.DEATHSCREEN);
        }

        if (m_currentUIState != UIStates.HACKING && m_currentUIState != UIStates.NONE)
        {
            m_txtTimer.text = Timer.Instance.TimerValue.ToString("F3");
        }
    }
}
