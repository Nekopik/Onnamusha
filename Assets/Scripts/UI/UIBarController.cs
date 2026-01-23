using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    public Image hpBarFill;
    public Image expBarFill;

    private PlayerStats playerStats;

    private void Start()
    {
        // Find the player and get PlayerStats component
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component not found on Player GameObject!", this);
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'Player' not found!", this);
        }

        if (hpBarFill == null)
        {
            Debug.LogError("HP Bar Fill Image not assigned in Inspector!", this);
        }
        if (expBarFill == null)
        {
            Debug.LogError("EXP Bar Fill Image not assigned in Inspector!", this);
        }
    }

    private void Update()
    {
        if (playerStats != null && hpBarFill != null)
        {
            // Ensure float division by casting
            float healthRatio = (float)playerStats.currentHealth / playerStats.maxHealth;
            hpBarFill.fillAmount = Mathf.Clamp01(healthRatio);

            // Log values to help debug
            //Debug.Log($"Current Health: {playerStats.currentHealth}, Max Health: {playerStats.maxHealth}");
            //Debug.Log($"Calculated Health Ratio: {healthRatio}, Set Fill Amount: {hpBarFill.fillAmount}");

            // EXP bar is explicitly 0.0f
            expBarFill.fillAmount = 0.0f;
        }
        // If playerStats or hpBarFill is null, the above block won't run, and error messages from Start will appear.
    }
}