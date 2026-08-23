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

    public void TakeDamage(float _damageValue)
    {
        m_currentHealth -= _damageValue;

        if(m_currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharController>() != null)
        {
            if (m_isWeakeningNeeded == true)
            {
                if (m_currentHealth <= m_maxHealth * m_lidThreshold)
                {
                    m_isLidOpenable = true;
                    UIManager.Instance.EnableHackingIndicator();
                    HackingManager.Instance.SetCurrentHackableEnemy(this);
                }
            }
            else
            {
                m_isLidOpenable = true;
                UIManager.Instance.EnableHackingIndicator();
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
            UIManager.Instance.DisableHackingIndicator();
            Debug.Log("Lid Not Openable");
        }
    }
}
