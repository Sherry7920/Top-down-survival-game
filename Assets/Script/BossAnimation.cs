using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    BossFSM bossFSM;

    bool wasAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossFSM = GetComponent<BossFSM>();
    }

    void Update()
    {
        if (bossFSM == null) return;

        HandleMovementAnimation();
        HandleFlip();
        HandleAttackAnimation();
    }

    // =========================
    // Movement Animation
    // =========================
    void HandleMovementAnimation()
    {
        bool isMoving =
            bossFSM.currentState == BossFSM.State.Chase;

        animator.SetBool("isMoving", isMoving);
    }

    // =========================
    // Flip Boss Left / Right
    // =========================
    void HandleFlip()
    {
        if (!bossFSM) return;

        Transform player =
            GameObject.FindWithTag("Player")?.transform;

        if (!player) return;

        float dir = player.position.x - transform.position.x;

        if (dir < -0.01f)
            spriteRenderer.flipX = true;
        else if (dir > 0.01f)
            spriteRenderer.flipX = false;
    }

    // =========================
    // Attack Trigger (Safe Version)
    // =========================
    void HandleAttackAnimation()
    {
        bool isAttacking =
            bossFSM.currentState == BossFSM.State.Attack;

        // Trigger only once when entering Attack state
        if (isAttacking && !wasAttacking)
        {
            animator.SetTrigger("Attack");
        }

        wasAttacking = isAttacking;
    }
}