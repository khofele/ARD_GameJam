using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovement : CharMovement
{
    private const float m_charSpeed = 0.45f;

    private Vector2 m_humanMoveInput = Vector2.zero;
    private Vector2 m_humanLookInput = Vector2.zero;

    [SerializeField] private CharacterController m_characterController = null;

    private void OnMoveHuman(InputValue _value)
    {
        m_humanMoveInput = _value.Get<Vector2>();
    }

    private void OnLookHuman(InputValue _value)
    {
        m_humanLookInput = _value.Get<Vector2>();
    }

    private void OnExecuteHackingHuman(InputValue _value)
    {
        if(_value.isPressed == true)
        {
            HackingManager.Instance.TriggerHacking();
        }
    }

    private void Move()
    {
        Vector3 normalizedMovementVector = new Vector3(m_humanMoveInput.x, 0.0f, m_humanMoveInput.y).normalized;
        Vector3 transformedMovementVector = transform.TransformDirection(normalizedMovementVector);

        m_characterController.Move(transformedMovementVector * m_charSpeed * Time.deltaTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * m_humanLookInput.x * 0.05f); // TODO balance camera sensitivity modifier
    }

    public override void UpdateMovement()
    {
        Move();
        Look();
    }
}
