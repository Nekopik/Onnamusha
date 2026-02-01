using UnityEngine;

public class PlayerBossSpawn : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.SetBossPlayerSpawnPoint();
            gameObject.SetActive(false);
        }
    }
}
