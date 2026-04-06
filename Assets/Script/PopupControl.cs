using UnityEngine;
using TMPro; // Required for TextMesh Pro buttons/text

public class PopupControl : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Drag the GameOver Panel here")]
    public GameObject gameOverPanel;

    [Header("Win Settings (Optional)")]
    [Tooltip("Drag the Win Panel here (Only needed for Level 2)")]
    public GameObject winPanel;

    void Awake()
    {
        // Automatically hide both panels when the level starts
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    // --- Methods to show the popups ---

    // Called by GameManager when player dies
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // Called by GameManager when player wins Level 2
    public void ShowWinPopup()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // --- Button Actions (Linked in Inspector) ---

    // For "Retry" Button: Restarts the current scene
    public void OnRetryButtonClicked()
    {
        Time.timeScale = 1f; // IMPORTANT: Reset time before loading
        GameManager.Instance.RestartCurrentLevel();
    }

    // For "Try Again" Button: Specifically goes back to Level 1
    public void OnBackToLevel1Clicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadLevel("Level1");
    }

    // For "Exit" Button: Quits the application
    public void OnExitClicked()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}