using UnityEngine;

public class BossFightStartTrigger : MonoBehaviour
{
    [SerializeField] private Boss_AIBrain boss_AIBrain;
    [SerializeField] private GameObject wall;
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerStats playerStats;

    private Oni oni;

    public float PlayerBossFightStartHP;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Wall appears");
            boss_AIBrain.StartBossFight();
            boss.SetBossFightActive();
            PlayerBossFightStartHP = playerStats.GetHPPercent();
            wall.SetActive(true);
        }
    }

    private void Update()
    {
        if (playerStats == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerStats = player.GetComponent<PlayerStats>();
            }
            return;
        }
    }

    public void SetWallInactive()
    {
        wall.SetActive(false);
    }
}
