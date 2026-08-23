using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected float m_currentHealth = 0.0f;
    protected bool m_isLidOpenable = false;

    [Header("General Enemy Settings")]
    [SerializeField] protected CharStates m_correspondingCharState = CharStates.DEFAULT_HUMAN;
    [SerializeField] protected bool m_isWeakeningNeeded = false;
    [Tooltip("Only relevant when weakening is needed")] [SerializeField] protected float m_lidThreshold = 0.2f;
    [SerializeField] protected float m_maxHealth = 100.0f;

    public bool IsLidOpenable
    {
        get { return m_isLidOpenable; }
    }

    public CharStates CorrespondingCharState
    {
        get { return m_correspondingCharState; }
    }

    protected virtual void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    protected void OnTriggerEnter(Collider other) // TODO child classes/prefabs need trigger!!
    {
        if (other.gameObject.GetComponent<CharController>() != null)
        {
            if (m_isWeakeningNeeded == true)
            {
                if (m_currentHealth <= m_maxHealth * m_lidThreshold)
                {
                    m_isLidOpenable = true; // TODO Lid can be opened --> display E-Button (UI-Component, referenced in Enemy class)
                    HackingManager.Instance.SetCurrentHackableEnemy(this);
                }
            }
            else
            {
                m_isLidOpenable = true;
                HackingManager.Instance.SetCurrentHackableEnemy(this);
                Debug.Log("Lid Openable");
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharController>() != null)
        {
            m_isLidOpenable = false;
            Debug.Log("Lid Not Openable");
        }
    }
}
