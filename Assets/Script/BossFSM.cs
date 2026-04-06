using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    public State currentState = State.Idle;

    [Header("Ranges")]
    public float chaseRange = 7f;
    public float attackRange = 3f;

    [Header("Base Speed")]
    public float baseSpeed = 3f;

    Transform player;

    public BossController bossController;
    public BossAttack attack;

    bool rageTriggered = false;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<BossAttack>();
        bossController = GetComponent<BossController>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;

            PlayerHealth ph = p.GetComponent<PlayerHealth>();
            if (ph != null)
                attack.playerHealth = ph;
        }
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // SAFE ROAD
        PlayerGroundState ground = player.GetComponent<PlayerGroundState>();
        if (ground != null && ground.IsOnSafeRoad)
        {
            agent.isStopped = true;
            return;
        }

        int hearts = bossController.heartsCollected;

        // Trigger Rage Music using AudioManager
        if (!rageTriggered && hearts >= 4)
        {
            rageTriggered = true;

            Debug.Log("[BossFSM] Rage reached → Triggering AudioManager");

            if (AudioManager.Instance != null)
                AudioManager.Instance.TriggerRage();
        }

        // ---------- STATE LOGIC ----------

        if (hearts < 2)
        {
            currentState = State.Idle;
        }
        else
        {
            if (dist <= attackRange)
                currentState = State.Attack;
            else if (dist <= chaseRange)
                currentState = State.Chase;
            else
                currentState = State.Idle;
        }

        switch (currentState)
        {
            case State.Idle:
                agent.isStopped = true;
                break;

            case State.Chase:
                HandleChase();
                break;

            case State.Attack:
                agent.isStopped = true;
                attack.TryAttack(GetAttackMultiplier());
                break;
        }
    }

    void HandleChase()
    {
        agent.isStopped = false;

        float speedMultiplier = GetSpeedMultiplier();
        agent.speed = baseSpeed * speedMultiplier;

        agent.SetDestination(player.position);

        float dashChance = GetDashChance();

        if (Random.value < dashChance)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Dash(dir);
        }
    }

    void Dash(Vector2 dir)
    {
        Vector3 dashTarget = transform.position + (Vector3)dir * 3f;
        agent.speed = baseSpeed * 2f;
        agent.SetDestination(dashTarget);
    }

    // ================================
    // Difficulty Curve
    // ================================

    float GetSpeedMultiplier()
    {
        int hearts = bossController.heartsCollected;

        switch (hearts)
        {
            case 0:
            case 1:
            case 2:
                return 0.6f;

            case 3:
                return 0.8f;

            case 4:
                return 1.0f;

            case 5:
                return 1.4f;

            default:
                return 1.4f;
        }
    }

    float GetAttackMultiplier()
    {
        int hearts = bossController.heartsCollected;

        switch (hearts)
        {
            case 0:
            case 1:
            case 2:
                return 0.5f;

            case 3:
                return 0.6f;

            case 4:
                return 0.7f;

            case 5:
                return 1.5f;

            default:
                return 1.5f;
        }
    }

    float GetDashChance()
    {
        int hearts = bossController.heartsCollected;

        switch (hearts)
        {
            case 4:
                return 0.01f;
            case 5:
                return 0.02f;
            default:
                return 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}