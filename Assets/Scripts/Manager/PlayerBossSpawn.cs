using UnityEngine;

public class PlayerBossSpawn : MonoBehaviour
{
    //previous respawn point, now recovering health
    private GameManager gameManager;
    [SerializeField] PlayerStats playerStats;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            //gameManager.SetBossPlayerSpawnPoint();
            //gameObject.SetActive(false);
            playerStats.currentHealth = playerStats.maxHealth;
            Debug.Log("Recovered HP");
        }
        
    }
}
