using UnityEngine;

public class BossEmotion : MonoBehaviour
{
    [Range(0, 100)]
    public float frustration = 0f;

    public float speedMultiplier = 0.2f;
    public float attackRateMultiplier = 0.3f;

    public void AddFrustration(float amount)
    {
        frustration = Mathf.Clamp(frustration + amount, 0, 100);
        UpdateMultipliers();
        Debug.Log($"[Emotion] Frustration = {frustration} | Speed x{speedMultiplier:F2} | Attack x{attackRateMultiplier:F2}");
    }

    void UpdateMultipliers()
    {
        speedMultiplier = 1f + frustration / 100f;
        attackRateMultiplier = 1f + frustration / 150f;
    }
}