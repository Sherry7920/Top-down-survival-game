using UnityEngine;
using TMPro; // Required for TextMesh Pro components

public class GameOverPopup : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject popupPanel; // Drag your 'GameOverPanel' here in the Inspector

    void Awake()
    {
        // Ensure the popup is hidden when the level starts
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    // This is called by the GameManager when the player loses
    public void ShowPopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true); // Show the UI
            Time.timeScale = 0f;        // Pause the game physics and movement
        }
    }

    // Link this method to your 'Retry Button' OnClick event in the Inspector
    public void OnRetryButtonClicked()
    {
        Debug.Log("Retry button clicked!");
        GameManager.Instance.RestartCurrentLevel();
    }
}