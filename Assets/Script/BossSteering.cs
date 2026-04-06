using UnityEngine;

public class BossSteering : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 1.5f;
    public LayerMask obstacleLayer;

    [Header("Steering")]
    public float steerAngle = 90f;
    public float steerLockTime = 0.4f;

    Vector2 lockedDir;
    float lockTimer;

    public Vector2 GetSteeredDirection(Vector2 desiredDir)
    {
        // ⭐ 如果正在绕墙 → 直接用锁定方向
        if (lockTimer > 0f)
        {
            lockTimer -= Time.deltaTime;
            return lockedDir;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            desiredDir,
            rayDistance,
            obstacleLayer
        );

        Debug.DrawRay(transform.position, desiredDir * rayDistance, Color.red);

        if (!hit)
            return desiredDir;

        // ⭐ 只在“第一次撞到墙”时决定方向
        float side = Random.value > 0.5f ? 1f : -1f;

        Vector2 steerDir =
            Quaternion.Euler(0, 0, steerAngle * side) * desiredDir;

        lockedDir = steerDir.normalized;
        lockTimer = steerLockTime;

        Debug.Log("[Steering] Lock steer direction");

        Debug.DrawRay(transform.position, lockedDir * rayDistance, Color.yellow);
        return lockedDir;
    }
}