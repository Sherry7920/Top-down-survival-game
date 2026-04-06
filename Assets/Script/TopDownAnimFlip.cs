using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TopDownAnimatorController : MonoBehaviour
{
    private Animator animator;
    private TopDownMovement movement;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<TopDownMovement>();

        // Works even if SpriteRenderer is on a child
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 dir = movement.MoveDirection;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
        animator.SetFloat("Speed", dir.sqrMagnitude);
        animator.SetFloat("AbsX", Mathf.Abs(dir.x));

        // Flip when moving left
        if (dir.x < -0.01f)
            spriteRenderer.flipX = true;
        else if (dir.x > 0.01f)
            spriteRenderer.flipX = false;
    }
}