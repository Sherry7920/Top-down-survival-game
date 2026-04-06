using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public enum BossPhase
    {
        Observer,
        Predictor,
        Rage,
        Collapse
    }

    [Header("Progress")]
    public int heartsCollected = 0;

    [Header("Phase")]
    public BossPhase currentPhase = BossPhase.Observer;

    [Header("Final Survival")]
    public float survivalDuration = 10f;
    bool finalPhaseStarted = false;

    [Header("References")]
    public BossFSM fsm;

    void Awake()
    {
        if (!fsm)
            fsm = GetComponent<BossFSM>();
    }

    public void OnHeartCollected()
    {
        heartsCollected++;
        Debug.Log("Heart = " + heartsCollected);
        UpdatePhase();
    }

    void UpdatePhase()
    {
        BossPhase newPhase = BossPhase.Observer;

        if (heartsCollected >= 5)
            newPhase = BossPhase.Collapse;
        else if (heartsCollected >= 4)
            newPhase = BossPhase.Rage;
        else if (heartsCollected >= 3)
            newPhase = BossPhase.Predictor;
        else
            newPhase = BossPhase.Observer;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            Debug.Log("Phase -> " + currentPhase);

            if (currentPhase == BossPhase.Collapse && !finalPhaseStarted)
            {
                finalPhaseStarted = true;
                StartCoroutine(FinalSurvivalCountdown());
            }
        }
    }

    IEnumerator FinalSurvivalCountdown()
    {
        Debug.Log("🔥 FINAL SURVIVAL STARTED!");

        int timeLeft = Mathf.CeilToInt(survivalDuration);

        while (timeLeft > 0)
        {
            Debug.Log("⏳ Survival Countdown: " + timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        Debug.Log("⏳ Survival Countdown: 0");
        Debug.Log("🎉 SURVIVED! WIN!");

        GameManager.TryWin();
    }
}