using UnityEngine;
using UnityEngine.AI;

public class HammerRobotEnemy : Enemy
{
    private const float m_detectionRange = 10.0f; // TODO balance values
    private const float m_attackRange = 2.0f;
    private const float m_attackCooldownDuration = 2.0f;

    private HammerRobotStates m_currentHammerRobotState = HammerRobotStates.PATROL;
    private int m_currentPathIndex = 0;
    private float m_attackCooldownTimer = 0.0f;

    [Header("Hammer Robot References")]
    [SerializeField] private NavMeshAgent m_navAgent = null;
    [SerializeField] private Transform[] m_path = null;
    [SerializeField] private LayerMask m_playerLayer;

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
        Vector3 directionToChar = CharController.Instance.transform.position - transform.position;
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
        return Vector3.Distance(transform.position, CharController.Instance.transform.position);
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
        m_navAgent.SetDestination(CharController.Instance.transform.position);
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

        PerformAttack();
        // TODO implement attack animation
    }

    private void StartAttacking()
    {
        m_currentHammerRobotState = HammerRobotStates.ATTACK;
        m_navAgent.isStopped = true;
    }

    private void PerformAttack()
    {
        if(m_attackCooldownTimer > 0.0f)
        {
            return;
        }
        Debug.Log("Hammer Robot Enemy Attack");
        Vector3 attackCenter = transform.position + transform.forward;

        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, 1.2f, m_playerLayer);

        foreach (Collider hit in hitColliders)
        {
            CharController player = hit.GetComponent<CharController>();

            if (player != null)
            {
                Debug.Log(player.name);
                player.TakeDamage(15.0f);
            }
        }

        m_attackCooldownTimer = m_attackCooldownDuration;
    }

    private void UpdateAttackCooldown()
    {
        if (m_attackCooldownTimer > 0.0f)
        {
            m_attackCooldownTimer -= Time.deltaTime;
        }
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
            UpdateAttackCooldown();
        }
        else if(GameManager.Instance.CurrentGameState == GameStates.HACKING)
        {
            m_navAgent.isStopped = true;
        }
    }
}
