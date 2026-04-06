using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    // Public read-only direction (used by animator)
    public Vector2 MoveDirection { get; private set; }

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Disable gravity for top-down movement
        rb.gravityScale = 0f;

        // Prevent unwanted rotation
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get raw input from WASD / Arrow Keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Normalize to prevent faster diagonal movement
        MoveDirection = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        // Apply velocity for smooth physics movement
        rb.linearVelocity = MoveDirection * moveSpeed;
        //Debug.Log("Player pos = " + transform.position);
    }
}