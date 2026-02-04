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
            boss.SetBossFightInactive();
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

    private void Die()
    {
        GM.Respawn();
        Destroy(gameObject);
    }
}
