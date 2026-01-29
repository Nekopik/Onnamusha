using UnityEngine;

public class BossFightStartTrigger : MonoBehaviour
{
    [SerializeField] private Boss_AIBrain boss_AIBrain;
    [SerializeField] private GameObject wall;
    [SerializeField] private Boss boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Wall appears");
            boss_AIBrain.StartBossFight();
            boss.SetBossFightActive();
            wall.SetActive(true);
        }
    }
}
