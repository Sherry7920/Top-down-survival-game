using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MonsterFlocking))]
[RequireComponent(typeof(AudioSource))]
public class MonsterFSM : MonoBehaviour
{
    public enum State { Idle, Chase, Attack }
    public State CurrentState => currentState;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Ranges")]
    public float chaseRange = 7f;
    public float attackRange = 0.6f;

    [Header("Attack")]
    public float attackCooldown = 1f;

    [Header("Attack Lock")]
    public float attackLockDuration = 0.6f;

    [Header("Audio")]
    public AudioClip attackClip;

    Rigidbody2D rb;
    MonsterFlocking flocking;
    AudioSource audioSource;

    Transform target;
    PlayerGroundState groundState;
    PlayerHealth playerHealth;

    float nextAttackTime;
    float attackLockTimer;

    State currentState = State.Idle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        flocking = GetComponent<MonsterFlocking>();
        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
        {
            target = player.transform;
            groundState = target.GetComponent<PlayerGroundState>();
            playerHealth = target.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (!target) return;

        // SAFE ROAD
        if (groundState != null && groundState.IsOnSafeRoad)
        {
            currentState = State.Idle;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, target.position);

        // ATTACK LOCK (prevents jitter)
        if (attackLockTimer > 0f)
        {
            attackLockTimer -= Time.deltaTime;
            currentState = State.Attack;
        }
        else if (dist <= attackRange)
        {
            attackLockTimer = attackLockDuration;
            currentState = State.Attack;
        }
        else if (dist <= chaseRange + 0.5f) // hysteresis buffer
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                break;

            case State.Chase:
                flocking.CalculateChaseDirection();
                rb.linearVelocity =
                    flocking.CurrentDirection * moveSpeed;
                break;

            case State.Attack:
                rb.linearVelocity = Vector2.zero;
                HandleAttack();
                break;
        }
    }

    void HandleAttack()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        // Play attack sound
        if (attackClip && audioSource)
        {
            audioSource.PlayOneShot(attackClip);
        }

        if (playerHealth)
        {
            playerHealth.TakeDamage(1);
            Debug.Log(name + " ATTACK!");
        }
    }
}