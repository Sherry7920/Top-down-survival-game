using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Manually set your REAL visible map edges
    public float minX = -9f;
    public float maxX = 9f;
    public float minY = -8f;
    public float maxY = 5f;

    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(target.position.y, minY, maxY);

        Vector3 desiredPos = new Vector3(clampedX, clampedY, -10f);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );
    }
}