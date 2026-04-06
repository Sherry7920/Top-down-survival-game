using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Clips")]
    public AudioClip level1BGM;
    public AudioClip level2BGM;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    AudioSource audioSource;
    Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            PlayFresh(level1BGM, 0.3f);
        }
        else if (scene.name == "Level2")
        {
            PlayFresh(level2BGM, 0.5f);  // ALWAYS reset to 0.5
        }
    }

    // ==========================================
    // ALWAYS reset volume even if clip same
    // ==========================================
    void PlayFresh(AudioClip clip, float targetVolume)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRestart(clip, targetVolume));
    }

    IEnumerator FadeRestart(AudioClip clip, float targetVolume)
    {
        // 1️⃣ Fade out completely
        yield return StartCoroutine(FadeVolume(0f, fadeDuration * 0.5f));

        // 2️⃣ Force reset clip (even if same)
        audioSource.clip = clip;
        audioSource.Play();

        // 3️⃣ Force volume to 0 before fade in
        audioSource.volume = 0f;

        // 4️⃣ Fade in to target volume
        yield return StartCoroutine(FadeVolume(targetVolume, fadeDuration * 0.5f));
    }

    // ==========================================
    // Rage Volume Boost
    // ==========================================
    public void TriggerRage()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeVolume(0.9f, 2f));
    }

    // ==========================================
    // Core Fade
    // ==========================================
    IEnumerator FadeVolume(float target, float duration)
    {
        float start = audioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        audioSource.volume = target;
    }
}