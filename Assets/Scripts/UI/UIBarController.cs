using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    public Image hpBarFill;
    public Image expBarFill;

    [SerializeField] private PlayerStats playerStats;

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
        // If playerStats is missing or the object was destroyed, try to find the new player
        if (playerStats == null)
        {
            hpBarFill.fillAmount = 0f;
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerStats = player.GetComponent<PlayerStats>();
            }
            return; // wait until we have a valid player
        }

        // HP BAR
        float healthRatio = (float)playerStats.currentHealth / playerStats.maxHealth;
        hpBarFill.fillAmount = Mathf.Clamp01(healthRatio);

        // EXP BAR (still placeholder)
        expBarFill.fillAmount = 0.0f;
    }
}