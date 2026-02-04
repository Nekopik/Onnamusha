using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public float maxHealth, neededXP;

    public float currentHealth, currentXP;

    private Boss boss;
    private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        boss = GameObject.Find("Boss").GetComponent<Boss>();
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

    public void AddXP(float amount)
    {
        currentXP += amount;

        if(currentXP >= neededXP)
        {
            currentXP -= neededXP;
            //level++;
        }
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

        if (GM != null)
        {
            GM.Respawn(); // This triggers the timer in the GameManager
        }

        this.gameObject.SetActive(false);
    }

    // GameManager will call this right before turning the player back on
    public void ResetStats()
    {
        currentHealth = maxHealth;
        // If you use a Rigidbody2D, reset velocity so you don't spawn moving
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
