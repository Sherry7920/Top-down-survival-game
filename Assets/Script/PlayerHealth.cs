using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Audio")]
    public AudioClip deathClip;

    AudioSource audioSource;
    bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        Debug.Log("[PlayerHealth] Initialized. HP = " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("[PlayerHealth] Took damage: " + damage +
                  " | HP now: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        if (isDead) yield break;
        isDead = true;

        Debug.Log("[PlayerHealth] Player DIED");

        // 1. Play the death sound (if assigned)
        if (deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
            Debug.Log("[PlayerHealth] Playing death sound");
        }

        // 2. IMMEDIATELY show the Game Over popup
        GameManager.TryGameOver();

        yield return null;
    }
}