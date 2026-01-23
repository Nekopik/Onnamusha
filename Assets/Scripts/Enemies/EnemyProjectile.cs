using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private AttackDetails attackDetails;

    public void Fire(Vector2 direction, float speed, AttackDetails attackDetails)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.attackDetails = attackDetails;

    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.SendMessage("Damage", attackDetails);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
