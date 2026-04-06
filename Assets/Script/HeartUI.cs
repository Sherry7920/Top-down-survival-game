using TMPro;
using UnityEngine;

public class HeartUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    public PlayerCollector collector;

    void Update()
    {
        if (collector != null)
        {
            text.text =  collector.heartsCollected + " / " + collector.heartsNeeded;
        }
    }
}