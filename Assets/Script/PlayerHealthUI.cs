using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth player;

    public Image[] hearts; 

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.currentHealth)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}