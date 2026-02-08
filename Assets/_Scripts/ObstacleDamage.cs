using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        var health = col.collider.GetComponentInParent<PlayerHealth>();
        if (health) health.TakeDamage(damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var health = col.GetComponentInParent<PlayerHealth>();
        if (health) health.TakeDamage(damage);
    }
}
