using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    // CONST FIELDS
    private const string m_defaultHumanInputName = "Default_Human";
    private const string m_boxRobotInputName = "Box_Robot";
    private const string m_hammerRobotInputName = "Hammer_Robot";
    private const string m_serviceRobotInputName = "Service_Robot";
    private const string m_hackingInputName = "Hacking";
    private const float m_maxHealth = 100.0f;

    // PRIVATE FIELDS
    private CharStates m_currentCharState = CharStates.DEFAULT_HUMAN;
    private CharMovement m_currentMovement = null;
    private float m_currentHealth = 0.0f;

    // SERIALIZE FIELDS
    [Header("Movement References")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private HumanMovement m_humanMovement = null;
    [SerializeField] private BoxRobotMovement m_boxRobotMovement = null;
    [SerializeField] private HammerRobotMovement m_hammerRobotMovement = null;
    [SerializeField] private ServiceRobotMovement m_serviceRobotMovement = null;

    public static CharController Instance
    {
        get; private set;
    }

    public CharStates CurrentCharState
    {
        get { return m_currentCharState; }
    }

    public void SetCharState(CharStates _newCharState)
    {
        m_currentCharState = _newCharState;
        m_playerInput.SwitchCurrentActionMap(GetCurrentPlayerInputMap());
        m_currentMovement = GetCurrentStateMovement();

        Debug.Log("Char State changed to " + _newCharState);
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private string GetCurrentPlayerInputMap()
    {
        switch (m_currentCharState)
        {
            case CharStates.DEFAULT_HUMAN:
                return m_defaultHumanInputName;

            case CharStates.BOX_ROBOT:
                return m_boxRobotInputName;

            case CharStates.HAMMER_ROBOT:
                return m_hammerRobotInputName;

            case CharStates.SERVICE_ROBOT:
                return m_serviceRobotInputName;

            default:
                return m_defaultHumanInputName;
        }
    }

    private CharMovement GetCurrentStateMovement()
    {
        switch (m_currentCharState)
        {
            case CharStates.DEFAULT_HUMAN:
                return m_humanMovement;

            case CharStates.BOX_ROBOT:
                return m_boxRobotMovement;

            case CharStates.HAMMER_ROBOT:
                return m_hammerRobotMovement;

            case CharStates.SERVICE_ROBOT:
                return m_serviceRobotMovement;

            default:
                return m_humanMovement;
        }
    }

    private void OnEnable()
    {
        m_playerInput.SwitchCurrentActionMap(GetCurrentPlayerInputMap());
    }

    private void OnDisable()
    {
        m_playerInput.DeactivateInput();
    }

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        SetCharState(CharStates.DEFAULT_HUMAN);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameStates.RUNNING)
        {
            if (m_currentMovement != null)
            {
                m_currentMovement.UpdateMovement();
            }
        }
        else if(GameManager.Instance.CurrentGameState == GameStates.HACKING)
        {
            m_playerInput.SwitchCurrentActionMap(m_hackingInputName);
        }
    }
}
