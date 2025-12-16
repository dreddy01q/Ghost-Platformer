using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public TextMeshProUGUI healthDisplay;
    public void updateHealthDisplay()
    {
        healthDisplay.text = "HP: "+playerHealth.Health;
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthDisplay();
    }
}
