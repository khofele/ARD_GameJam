using UnityEngine;
using UnityEngine.InputSystem;

public class BoxRobotMovement : CharMovement
{
    [SerializeField]
    private GameObject m_robotHead;

    public InputActionReference m_turnLeftInActRef;
    public InputActionReference m_turnRightInActRef;
    public InputActionReference m_reverseInActRef;
    public InputActionReference m_lookInActRef;

    [SerializeField]
    private const float m_forwardSpeed = 5.0f;

    private float m_gier = 0f;
    private float m_nick = 0f;
    [SerializeField]
    private float m_maxNick = 90f;
    [SerializeField]
    private float m_minNick = -90f;
    [SerializeField]
    private float m_degPerSec = 30f;

    [SerializeField] private CharacterController m_characterController = null;
    // TODO implement
    private void Move()
    {
        float reverseMod;
        if (m_reverseInActRef.action.IsPressed())
        {
            reverseMod = -1f;
        }
        else
        {
             reverseMod = 1f;
        }

        float horz = reverseMod * m_degPerSec * Time.deltaTime;

        if (m_turnLeftInActRef.action.IsPressed() && m_turnRightInActRef.action.IsPressed())
        {
            m_characterController.Move(transform.forward * m_forwardSpeed * Time.deltaTime);
        }
        else if (m_turnLeftInActRef.action.IsPressed()) // TODO Bugfix wird Kiste durch wand drücken
        {
            m_gier += horz;
        }
        else if (m_turnRightInActRef.action.IsPressed()) // TODO Bugfix wird Kiste durch wand drücken
        {
            m_gier -= horz;
        }

        transform.localRotation = Quaternion.AngleAxis(m_gier, Vector3.up);
    }
    private void Look()
    {
        Vector2 look = m_lookInActRef.action.ReadValue<Vector2>();

        float vert = look.y * Time.deltaTime * m_degPerSec;
        m_nick -= vert;
        m_nick = Mathf.Clamp(m_nick, m_minNick, m_maxNick);

        m_robotHead.transform.localRotation = Quaternion.AngleAxis(m_nick, Vector3.right);
    }
    public override void UpdateMovement()
    {
        Look();
        Move();
    }

    public void OnEnable()
    {
        m_turnLeftInActRef.action.Enable();
        m_turnRightInActRef.action.Enable();
        m_reverseInActRef.action.Enable();
        m_lookInActRef.action.Enable();
    }
    public void OnDisable() 
    {
        m_turnLeftInActRef.action.Disable();
        m_turnRightInActRef.action.Disable();
        m_reverseInActRef.action.Disable();
        m_lookInActRef.action.Disable();
    }
}
