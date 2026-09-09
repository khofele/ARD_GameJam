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
    [SerializeField]
    private float m_currentHealth = 0.0f;

    // SERIALIZE FIELDS
    [Header("Movement References")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private HumanMovement m_humanMovement = null;
    [SerializeField] private BoxRobotMovement m_boxRobotMovement = null;
    [SerializeField] private HammerRobotMovement m_hammerRobotMovement = null;
    [SerializeField] private ServiceRobotMovement m_serviceRobotMovement = null;

    //Aussehen und Größe
    [SerializeField] private GameObject m_boxRobot = null;
    [SerializeField] private GameObject m_hammerRobot = null;
    [SerializeField] private GameObject m_serviceRobot = null;
    [SerializeField] private GameObject m_defaultHuman = null;

    private CharacterController m_characterController;
    private Vector3 m_velocity = Vector3.zero;
    private float m_gravity = -9.81f;
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

        SetVisuals();

        Debug.Log("Char State changed to " + _newCharState);
    }

    public void TakeDamage(float _damageValue)
    {
        m_currentHealth -= _damageValue;
        Debug.Log(m_currentHealth);
        if (m_currentHealth <= 0.0f)
        {
            GameManager.Instance.SetGameState(GameStates.GAMEOVER);
        }
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // damit resettet wird

        m_characterController = GetComponent<CharacterController>();
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

    private void SetVisuals()
    {
        switch (m_currentCharState)
        {
            //case CharStates.DEFAULT_HUMAN:
            //    m_defaultHuman.SetActive(true);
            //    m_boxRobot.SetActive(false);
            //    m_hammerRobot.SetActive(false);
            //    m_serviceRobot.SetActive(false);
            //break;

            case CharStates.BOX_ROBOT:
                m_defaultHuman.SetActive(false);
                m_boxRobot.SetActive(true);
                m_hammerRobot.SetActive(false);
                m_serviceRobot.SetActive(false);
                m_characterController.radius = 0.15f;
                m_characterController.height = 0.4f;
                m_characterController.center = new Vector3(0f, 0f, 0f);
                break;

            case CharStates.HAMMER_ROBOT:
                m_defaultHuman.SetActive(false);
                m_boxRobot.SetActive(false);
                m_hammerRobot.SetActive(true);
                m_serviceRobot.SetActive(false);
                m_characterController.radius = 0.15f;
                m_characterController.height = 1.15f;
                m_characterController.center = new Vector3(0f, 0.375f, 0f);
                break;

            case CharStates.SERVICE_ROBOT:
                m_defaultHuman.SetActive(false);
                m_boxRobot.SetActive(false);
                m_hammerRobot.SetActive(false);
                m_serviceRobot.SetActive(true);
                m_characterController.radius = 0.15f;
                m_characterController.height = 1.15f;
                m_characterController.center = new Vector3(0f, 0.375f, 0f);
                break;

            default:
                m_defaultHuman.SetActive(true);
                m_boxRobot.SetActive(false);
                m_hammerRobot.SetActive(false);
                m_serviceRobot.SetActive(false);
                m_characterController.radius = 0.1f;
                m_characterController.height = 0.4f;
                m_characterController.center = new Vector3(0f, 0f, 0f);
            break;
        }
    }
    private void ProcessGravity()
    {
        if (m_characterController.isGrounded && m_velocity.y <= 0f)
        {
            m_velocity.y = -2f;
        }
        m_velocity.y += m_gravity * Time.deltaTime * Time.deltaTime;
        m_characterController.Move(m_velocity);
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
            ProcessGravity();
        }
        else if(GameManager.Instance.CurrentGameState == GameStates.HACKING)
        {
            m_playerInput.SwitchCurrentActionMap(m_hackingInputName);
        }

        //if(m_currentHealth <= 0.0f)
        //{
        //    GameManager.Instance.SetGameState(GameStates.GAMEOVER);
        //}
    }

    public void TakeTransform(Transform enemyTransform)
    {
        m_characterController.enabled = false;
        m_characterController.transform.position = enemyTransform.position;
        m_characterController.transform.rotation = enemyTransform.rotation;
        m_characterController.enabled = true;
    }
}
