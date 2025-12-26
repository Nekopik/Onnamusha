using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    private AttackDetails attackDetails;
    private Rigidbody2D rb;

    public void Setup(AttackDetails details, int direction)
    {
        attackDetails = details;

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed * direction, 0);

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.parent.SendMessage("Damage", attackDetails);
        }

        Destroy(gameObject);
    }
}
