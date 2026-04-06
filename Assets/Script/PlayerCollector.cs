using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerCollector : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap heartTilemap;

    [Header("Boss Reference (Assign in Level2 only)")]
    public BossController boss;

    [Header("Sound")]
    public AudioClip collectCoinSound;
    private AudioSource audioSource;

    [Header("Hearts")]
    public int heartsCollected = 0;
    public int heartsNeeded = 5;

    Vector3Int currentCell;
    bool canCollect = false;

    // Prevent double-trigger when pressing E quickly
    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (isTransitioning) return;

        if (canCollect && Input.GetKeyDown(KeyCode.E))
        {
            CollectHeart();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (heartTilemap != null && other.gameObject == heartTilemap.gameObject)
        {
            UpdateCurrentCell();
            canCollect = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (heartTilemap != null && other.gameObject == heartTilemap.gameObject)
        {
            UpdateCurrentCell();
            canCollect = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (heartTilemap != null && other.gameObject == heartTilemap.gameObject)
        {
            canCollect = false;
        }
    }

    void UpdateCurrentCell()
    {
        Vector3 worldPos = transform.position;
        currentCell = heartTilemap.WorldToCell(worldPos);
    }

    void CollectHeart()
    {
        if (heartTilemap == null) return;
        if (!heartTilemap.HasTile(currentCell)) return;

        // Remove heart tile
        heartTilemap.SetTile(currentCell, null);
        heartsCollected++;

        // Play SFX
        if (collectCoinSound != null)
            audioSource.PlayOneShot(collectCoinSound);

        Debug.Log($"Heart Collected: {heartsCollected}/{heartsNeeded}");

        // Level2: notify boss (starts phases / final survival)
        if (boss != null)
        {
            boss.OnHeartCollected();
        }

        // If reached target hearts, we may need to transition (Level1)
        // AND we also want the last collect sound to finish (Level1 + Level2).
        if (heartsCollected >= heartsNeeded)
        {
            StartCoroutine(HandleReachedHearts());
        }
    }

    IEnumerator HandleReachedHearts()
    {
        isTransitioning = true;

        // Wait for the collect sound to finish (if any)
        if (collectCoinSound != null)
            yield return new WaitForSeconds(collectCoinSound.length);

        // Level1: no boss -> go next level immediately
        // Level2: boss exists -> do NOT win here (boss will handle 10s survival -> win)
        if (boss == null)
        {
            GameManager.TryWin();
        }

        // Allow input again (mainly for Level2; Level1 will load a new scene anyway)
        isTransitioning = false;
    }
}