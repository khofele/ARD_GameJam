using UnityEngine;
using UnityEngine.AI;

public class HammerRobotEnemy : Enemy
{
    private const float m_detectionRange = 10.0f; // TODO balance values
    private const float m_attackRange = 2.0f;

    private HammerRobotStates m_currentHammerRobotState = HammerRobotStates.PATROL;
    private int m_currentPathIndex = 0;

    [Header("Hammer Robot References")]
    [SerializeField] private NavMeshAgent m_navAgent = null;
    [SerializeField] private CharController m_charController = null;
    [SerializeField] private Transform[] m_path = null;

    private void ExecuteEnemyBehavior()
    {
        switch(m_currentHammerRobotState)
        {
            case HammerRobotStates.PATROL:
                Patrol();
                break;

            case HammerRobotStates.CHASE:
                Chase();
                break;

            case HammerRobotStates.ATTACK:
                Attack();
                break;
        }
    }

    private bool IsCharacterInVisibleAngle()
    {
        Vector3 directionToChar = m_charController.transform.position - transform.position;
        float distanceToChar = CalculateCharacterDistance();

        if (distanceToChar > m_detectionRange)
        {
            return false;
        }

        directionToChar.Normalize();

        float angleEnemyChar = Vector3.Angle(transform.forward, directionToChar);

        if(angleEnemyChar <= 90.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float CalculateCharacterDistance()
    {
        return Vector3.Distance(transform.position, m_charController.transform.position);
    }
    // PATROL ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Patrol()
    {
        if(IsCharacterInVisibleAngle() == true)
        {
            StartChasing();
            return;
        }

        if(m_navAgent.pathPending == false && m_navAgent.remainingDistance <= m_navAgent.stoppingDistance) // stopping distance needs to be above 0 to ensure smooth patroling
        {
            m_currentPathIndex++;

            if(m_currentPathIndex >= m_path.Length)
            {
                // loop path
                m_currentPathIndex = 0;
            }

            TraversePath();
        }
    }

    private void TraversePath()
    {
        if(m_path.Length == 0)
        {
            Debug.LogError("No Path defined!");
            return;
        }

        m_navAgent.SetDestination(m_path[m_currentPathIndex].position);
    }

    private void StartPatroling()
    {
        m_currentHammerRobotState = HammerRobotStates.PATROL;
        m_navAgent.isStopped = false; // set whether nav agent stops or continues movement along set path

        TraversePath();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // CHASE ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Chase()
    {
        if(IsCharacterInVisibleAngle() == false)
        {
            StartPatroling();
            return;
        }

        // attack if char in range and in sight
        if(CalculateCharacterDistance() <= m_attackRange && IsCharacterInVisibleAngle() == true)
        {
            StartAttacking();
            return;
        }

        // execute chasing
        m_navAgent.isStopped = false;
        m_navAgent.SetDestination(m_charController.transform.position);
    }

    private void StartChasing()
    {
        m_currentHammerRobotState = HammerRobotStates.CHASE;
        m_navAgent.isStopped = false;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ATTACK ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Attack()
    {
        m_navAgent.isStopped = true;

        if(CalculateCharacterDistance() > m_attackRange && IsCharacterInVisibleAngle() == true)
        {
            StartChasing();
            return;
        }

        if(IsCharacterInVisibleAngle() == false)
        {
            StartPatroling();
            return;
        }

        Debug.Log("Attack");
        // TODO implement alternating attack + anim
    }

    private void StartAttacking()
    {
        m_currentHammerRobotState = HammerRobotStates.ATTACK;
        m_navAgent.isStopped = true;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected override void Start()
    {
        base.Start();
        StartPatroling();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameStates.RUNNING)
        {
            ExecuteEnemyBehavior();
        }
        else if(GameManager.Instance.CurrentGameState == GameStates.HACKING)
        {
            m_navAgent.isStopped = true;
        }
    }
}
