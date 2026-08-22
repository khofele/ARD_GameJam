using UnityEngine;
using UnityEngine.AI;

public class BoxRobotEnemy : Enemy
{
    private int m_currentPathIndex = 0;

    [Header("Box Robot References")]
    [SerializeField] private NavMeshAgent m_navAgent = null;
    [SerializeField] private bool m_isFollowingCharacter = false; // set true if box robot should follow char, set false if box robot should follow a set path
    [Tooltip("Only relevant when robot follows character")] [SerializeField] private CharController m_charController = null;
    [Tooltip("Only relevant when robot follows a set path")] [SerializeField] private Transform[] m_path = null;

    private void Move()
    {
        if(m_isFollowingCharacter == false)
        {
            if (m_path.Length == 0)
            {
                Debug.LogError("No Path defined!");
                return;
            }

            if (m_currentPathIndex < m_path.Length)
            {
                m_navAgent.SetDestination(m_path[m_currentPathIndex].position);
            }

            if (m_navAgent.pathPending == false && m_navAgent.remainingDistance <= m_navAgent.stoppingDistance) // stopping distance needs to be above 0 to ensure smooth movement
            {
                m_currentPathIndex++;
            }
        }
        else
        {
            m_navAgent.SetDestination(m_charController.transform.position);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameStates.RUNNING)
        {
            Move();
        }
    }
}
