using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected float m_maxHealth = 100.0f;
    protected float m_currentHealth = 0.0f;
    protected float m_lidThreshold = 0.2f;
    protected bool m_isLidOpenable = false;
    protected bool m_isLidOpen = false;

    [SerializeField] protected bool m_isWeakeningNeeded = false;

    protected virtual void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    protected void OnTriggerEnter(Collider other) // TODO child classes/prefabs need trigger!!
    {
        if(other.gameObject.GetComponent<CharController>() != null)
        {
            if (m_isWeakeningNeeded == true)
            {
                if (m_currentHealth <= m_maxHealth * m_lidThreshold)
                {
                    m_isLidOpenable = true; // TODO Lid can be opened --> display Button, player needs to press E or so --> set isLidOpen = true --> Starts Minigame, stops enemy ai
                    // TODO maybe: add property for corresponding char state after hacking
                }
            }
            else
            {
                m_isLidOpenable = true;
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharController>() != null)
        {
            m_isLidOpenable = false;
        }
    }
}
