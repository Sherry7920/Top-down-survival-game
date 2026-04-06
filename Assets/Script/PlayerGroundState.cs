using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerGroundState : MonoBehaviour
{
    public bool IsOnSafeRoad { get; private set; }

    [Header("Safe Road Sound")]
    public AudioClip safeRoadSound;

    [Header("Final Phase Settings")]
    public float safeDuration = 3f;      // 3 seconds safe
    public float unsafeDuration = 1f;    // 1 second unsafe

    AudioSource audioSource;
    BossController boss;

    bool isInsideRoad = false;
    bool finalModeActive = false;
    Coroutine safeRoutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boss = FindFirstObjectByType<BossController>();

        Debug.Log("[GroundState] Initialized.");
    }

    void Update()
    {
        if (boss == null) return;

        // Detect Final Phase
        if (!finalModeActive &&
            boss.currentPhase == BossController.BossPhase.Collapse)
        {
            finalModeActive = true;
            Debug.Log("[GroundState] FINAL PHASE ACTIVATED → Road will flash SAFE/UNSAFE");

            if (isInsideRoad)
                StartSafeCycle();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer ==
            LayerMask.NameToLayer("Road"))
        {
            isInsideRoad = true;

            Debug.Log("[GroundState] Player ENTERED Road");

            if (!finalModeActive)
            {
                IsOnSafeRoad = true;
                Debug.Log("[GroundState] SAFE (Normal Mode)");
                PlaySafeRoadSound();
            }
            else
            {
                StartSafeCycle();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer ==
            LayerMask.NameToLayer("Road"))
        {
            isInsideRoad = false;
            IsOnSafeRoad = false;

            Debug.Log("[GroundState] Player EXITED Road → UNSAFE");

            if (safeRoutine != null)
                StopCoroutine(safeRoutine);
        }
    }

    // ======================================
    // Safe Flash Cycle (3s safe / 1s unsafe)
    // ======================================
    void StartSafeCycle()
    {
        if (safeRoutine != null)
            StopCoroutine(safeRoutine);

        Debug.Log("[GroundState] Starting SAFE cycle");

        safeRoutine = StartCoroutine(SafeCycle());
    }

    IEnumerator SafeCycle()
    {
        while (isInsideRoad)
        {
            // SAFE PERIOD
            IsOnSafeRoad = true;
            Debug.Log("[GroundState] SAFE (Flashing Mode)");
            PlaySafeRoadSound();
            yield return new WaitForSeconds(safeDuration);

            // UNSAFE PERIOD
            IsOnSafeRoad = false;
            Debug.Log("[GroundState] UNSAFE (Flashing Mode)");
            yield return new WaitForSeconds(unsafeDuration);
        }
    }

    void PlaySafeRoadSound()
    {
        if (safeRoadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(safeRoadSound);
        }
    }
}