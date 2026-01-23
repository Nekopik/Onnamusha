using UnityEngine;

public class BossFightStartTrigger : MonoBehaviour
{
    [SerializeField] private Boss_AIBrain boss_AIBrain;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss_AIBrain.StartBossFight();
            gameObject.SetActive(false);
        }
    }
}
