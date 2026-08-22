using UnityEngine;
using UnityEngine.InputSystem;

public class HammerRobotMovement : CharMovement
{
    private const float m_charSpeed = 1.0f; // TODO balance values
    private const float m_attackLongCooldown = 3.0f;
    private const float m_attackAlternatingCooldown = 1.5f;

    private Vector2 m_hammerRobotMoveInput = Vector2.zero;
    private Vector2 m_hammerRobotLookInput = Vector2.zero;
    private float m_totalMouseLookMovement = 0.0f;
    private float m_attackCooldownTimer = 0.0f;
    private bool m_isLastAttackLeft = false;

    [SerializeField] private CharacterController m_characterController = null;

    private void OnMoveHammerRobot(InputValue _value)
    {
        m_hammerRobotMoveInput = _value.Get<Vector2>();
    }

    private void OnLookHammerRobot(InputValue _value)
    {
        m_hammerRobotLookInput = _value.Get<Vector2>();
    }

    private void OnAttackLeftHammerRobot(InputValue _value)
    {
        if(_value.isPressed == true)
        {
            Attack(true);
        }
    }

    private void OnAttackRightHammerRobot(InputValue _value)
    {
        if (_value.isPressed == true)
        {
            Attack(false);
        }
    }

    private void OnExecuteHackingHammerRobot(InputValue _value)
    {
        if (_value.isPressed == true)
        {
            HackingManager.Instance.ExecuteHacking();
        }
    }

    private void Attack(bool _isAttackLeft)
    {
        if(m_attackCooldownTimer > 0.0f)
        {
            return;
        }

        bool isAlternatingAttack = false;
        if(_isAttackLeft != m_isLastAttackLeft)
        {
            isAlternatingAttack = true;
        }

        if (_isAttackLeft == true)
        {
            Debug.Log("Left Attack"); // TODO implement animation + attack
        }
        else
        {
            Debug.Log("Right Attack"); // TODO implement animation + attack 
        }

        m_isLastAttackLeft = _isAttackLeft;

        if (isAlternatingAttack == true)
        {
            m_attackCooldownTimer = m_attackAlternatingCooldown;
        }
        else
        {
            m_attackCooldownTimer = m_attackLongCooldown;
        }
    }

    private void UpdateAttackCooldown()
    {
        if(m_attackCooldownTimer > 0.0f)
        {
            m_attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        Vector3 normalizedMovementVector = new Vector3(m_hammerRobotMoveInput.x, 0.0f, m_hammerRobotMoveInput.y).normalized;
        Vector3 transformedMovementVector = transform.TransformDirection(normalizedMovementVector);

        m_characterController.Move(transformedMovementVector * m_charSpeed * Time.deltaTime);
    }

    private void Look()
    {
        // accumulate mouse look input, if accumulated input is above threshold rotate camera by mouse look step
        m_totalMouseLookMovement += m_hammerRobotLookInput.x;

        float mouseThreshold = 50.0f;
        float mouseLookStep = 10.0f;

        if (Mathf.Abs(m_totalMouseLookMovement) >= mouseThreshold)
        {
            float direction = Mathf.Sign(m_totalMouseLookMovement);

            transform.Rotate(0.0f, direction * mouseLookStep, 0.0f);

            m_totalMouseLookMovement -= direction * mouseThreshold; // keep remaining mouse movement after rotation
        }
    }

    public override void UpdateMovement()
    {
        UpdateAttackCooldown();

        Move();
        Look();
    }
}
