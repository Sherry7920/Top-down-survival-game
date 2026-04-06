using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossAttack : MonoBehaviour
{
    [Header("Attack")]
    public float baseCooldown = 1f;
    float nextAttackTime;

    [Header("Player Reference")]
    public PlayerHealth playerHealth;

    [Header("Audio")]
    public AudioClip attackClip;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void TryAttack(float attackRateMultiplier)
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime =
            Time.time + baseCooldown / attackRateMultiplier;

        Debug.Log("[Attack] Boss ATTACK!");

        if (attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
            Debug.Log("[Attack] Player HP now: " + playerHealth.currentHealth);
        }
    }
}