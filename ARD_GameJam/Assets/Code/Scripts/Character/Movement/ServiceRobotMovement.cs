using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ServiceRobotMovement : CharMovement
{
    [SerializeField]
    private GameObject m_robotHead;
    //[SerializeField] private Slider m_moveChargeSlider;
    public float m_sliderValue = 0f;

    [Tooltip("Selber wert wie Hold-Wert in der InputAction")]
    public float m_secToFullyChargeSpeed = 1f;
    private bool m_isFullyCharged = false;
    [SerializeField]
    private float m_speed = 0f;
    [SerializeField]
    private float m_speedCharge = 0f;
    //[SerializeField] private float m_lowSpeedThreshhold = 0.25f;
    [SerializeField] private float m_maxSpeed = 10f;


    public InputActionReference m_lookInActRef;
    public InputActionReference m_chargeSpeedInActRef;
    public InputActionReference m_executeHackInActRef;

    private float m_gier = 0f;
    private float m_nick = 0f;
    [SerializeField]
    private float m_maxNick = 90f;
    [SerializeField]
    private float m_minNick = -90f;
    [SerializeField]
    private float m_degPerSec = 30f;

    [SerializeField] private CharacterController m_characterController = null;

    public AnimationCurve m_breakingCurve;
    
    private void Move()
    {
        if (m_speed <= 0f)//m_lowSpeedThreshhold)
        {
            m_speed = 0f;
            if (m_chargeSpeedInActRef.action.IsInProgress())
            {
                m_sliderValue += Time.deltaTime / m_secToFullyChargeSpeed;
                m_speedCharge += Time.deltaTime * (m_maxSpeed/m_secToFullyChargeSpeed);
            }
            if (m_chargeSpeedInActRef.action.WasPerformedThisFrame())
                m_isFullyCharged = true;
            if (m_chargeSpeedInActRef.action.WasReleasedThisFrame())
            {
                if (m_isFullyCharged)
                    m_speed = m_maxSpeed;
                else
                    m_speed = m_speedCharge;
                m_isFullyCharged = false;
                m_sliderValue = 0f;
                m_speedCharge = 0f;
            }
        }
        else
        {
            //m_speed = Mathf.MoveTowards(m_speed, 0f, ((0.004f * m_speed * m_speed) + 0.2f) * Time.deltaTime);//physikalisch korrekt aber langweilig
            //m_speed *= (1f-2f*Time.deltaTime);//2f*deltaTime sollte nicht >1 sonst rückwärts
            m_speed = Mathf.MoveTowards(m_speed, 0f, m_breakingCurve.Evaluate(Mathf.Abs(m_speed)) * Time.deltaTime);
        }

        m_characterController.Move(m_speed * Time.deltaTime * transform.forward);
    }
    private void Look()
    {
        Vector2 look = m_lookInActRef.action.ReadValue<Vector2>();

        float horz = look.x * Time.deltaTime * m_degPerSec;
        float vert = look.y * Time.deltaTime * m_degPerSec;
        m_gier += horz;
        m_nick -= vert;
        m_nick = Mathf.Clamp(m_nick, m_minNick, m_maxNick);

        transform.localRotation = Quaternion.AngleAxis(m_gier, Vector3.up);
        m_robotHead.transform.localRotation = Quaternion.AngleAxis(m_nick, Vector3.right);
    }

    private void ExecuteHacking()
    {
        if (m_executeHackInActRef.action.IsPressed())
        {
            HackingManager.Instance.TriggerHacking();
        }
    }

    public override void UpdateMovement()
    {
        Move();
        Look();
        ExecuteHacking();
    }

    private void OnEnable()
    {
        m_lookInActRef.action.Enable();
        m_chargeSpeedInActRef.action.Enable();
        m_executeHackInActRef.action.Enable();
    }
    private void OnDisable()
    {
        m_lookInActRef.action.Disable();
        m_chargeSpeedInActRef.action.Disable();
        m_executeHackInActRef.action.Disable();
    }
}
