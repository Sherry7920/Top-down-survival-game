using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MonsterAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    MonsterFSM monsterFSM;
    Rigidbody2D rb;

    bool wasAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterFSM = GetComponent<MonsterFSM>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!monsterFSM) return;

        HandleMovementAnimation();
        HandleFlip();
        HandleAttackAnimation();
    }

    // =========================
    // Movement Animation
    // =========================
    void HandleMovementAnimation()
    {
        bool isMoving = false;

        if (monsterFSM.CurrentState ==
            MonsterFSM.State.Chase)
        {
            // Only play Walk if velocity is meaningful
            if (rb.linearVelocity.magnitude > 0.1f)
                isMoving = true;
        }

        animator.SetBool("isMoving", isMoving);
    }

    // =========================
    // Flip Left / Right
    // =========================
    void HandleFlip()
    {
        float x = rb.linearVelocity.x;

        if (x < -0.05f)
            spriteRenderer.flipX = true;
        else if (x > 0.05f)
            spriteRenderer.flipX = false;
    }

    // =========================
    // Attack Trigger (Stable)
    // =========================
    void HandleAttackAnimation()
    {
        bool isAttacking =
            monsterFSM.CurrentState ==
            MonsterFSM.State.Attack;

        if (isAttacking && !wasAttacking)
        {
            animator.SetTrigger("Attack");
        }

        wasAttacking = isAttacking;
    }
}