using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float maxHealth;

    public float currentHealth;

    [SerializeField] GameObject playerDeadMenu;
    private Boss boss;
    [SerializeField] private Boss_AIBrain boss_AIBrain;
    private GameManager GM;

    private Oni_FightTracker mobFightTracker;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    private void Awake()
    {
        if (mobFightTracker == null)
            mobFightTracker = FindFirstObjectByType<Oni_FightTracker>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    public float GetHPPercent()
    {
        return currentHealth / maxHealth;
    }

    /*
    private void Die()
    {
        GM.Respawn();
        boss.SetBossFightInactive();
        Destroy(gameObject);
    }
    */

    private void Die()
    {
        if (boss != null) boss.SetBossFightInactive();

        mobFightTracker.EndFight();
        boss_AIBrain.playerHpLoss = 1f;
        boss_AIBrain.DecideSkillModifier();

        OpenEndGameMenu();

        this.gameObject.SetActive(false);


    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public void RecoverHP()
    {
        currentHealth = maxHealth;
    }

    public void OpenEndGameMenu()
    {
        Time.timeScale = 0f;
        playerDeadMenu.SetActive(true);
    }
}
