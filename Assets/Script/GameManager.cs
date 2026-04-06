using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance to allow other scripts to access the GameManager easily
    public static GameManager Instance;

    private void Awake()
    {
        // Check if an instance already exists to avoid duplicates
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene changes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // If the game starts in the "Bootstrap" scene, load the first level
        if (SceneManager.GetActiveScene().name == "Bootstrap")
        {
            LoadLevel("Level1");
        }
    }

    // ---------------------------------------------------------
    // STATIC WRAPPERS (Easy to call from any script)
    // ---------------------------------------------------------

    // Call this to trigger the Win logic from other scripts
    public static void TryWin()
    {
        if (Instance == null) return;
        Instance.WinGame();
    }

    // Call this to trigger the Game Over logic from other scripts
    public static void TryGameOver()
    {
        if (Instance == null) return;
        Instance.GameOver();
    }

    // ---------------------------------------------------------
    // CORE LOGIC
    // ---------------------------------------------------------

    // Changed to PUBLIC so BossController can access it
    public void WinGame()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("WinGame called in: " + currentScene);

        // If currently in Level 2, show the Win UI popup
        if (currentScene == "Level2")
        {
            PopupControl ui = Object.FindFirstObjectByType<PopupControl>();
            if (ui != null)
            {
                ui.ShowWinPopup();
            }
            else
            {
                Debug.LogError("WinGame: No PopupControl found in Level 2!");
            }
        }
        // If currently in Level 1, transition to Level 2
        else if (currentScene == "Level1")
        {
            LoadLevel("Level2");
        }
    }

    // Handles the Player's death and shows the Game Over UI
    public void GameOver()
    {
        Debug.Log("GAME OVER: Looking for PopupControl...");
        PopupControl ui = Object.FindFirstObjectByType<PopupControl>();

        if (ui != null)
        {
            ui.ShowGameOver();
        }
        else
        {
            Debug.LogError("No PopupControl found in scene!");
        }
    }

    // ---------------------------------------------------------
    // SCENE NAVIGATION
    // ---------------------------------------------------------

    // Restarts the current active scene and resets time
    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // Loads a specific scene by its name and resets time
    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}