using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    public Image hpBarFill;
    public Image shurikenCooldownBarFill;

    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        FindPlayerReferences();
    }

    private void Update()
    {
        if (playerStats == null || combatController == null)
        {
            hpBarFill.fillAmount = 0f;
            /*
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerStats = player.GetComponent<PlayerStats>();
            }
            */
            FindPlayerReferences();
            return;
        }

        if (combatController == null)
        {
            shurikenCooldownBarFill.fillAmount = 0f;
            PlayerCombatController combatController = GetComponent<PlayerCombatController>();
        }

        // HP BAR
        float healthRatio = (float)playerStats.currentHealth / playerStats.maxHealth;
        hpBarFill.fillAmount = Mathf.Clamp01(healthRatio);

        // shuriken cooldown BAR
        if (combatController != null)
        {
            shurikenCooldownBarFill.fillAmount = combatController.GetShurikenCooldown();
        }

        //Debug.Log("UI Fill: " + combatController.GetShurikenCooldown());

    }

    private void FindPlayerReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        playerStats = player.GetComponent<PlayerStats>();
        combatController = player.GetComponent<PlayerCombatController>();

        if (playerStats == null)
            Debug.LogError("PlayerStats missing!");

        if (combatController == null)
            Debug.LogError("PlayerCombatController missing!");
    }
}