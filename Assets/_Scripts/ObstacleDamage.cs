using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        var player = col.collider.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var player = col.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
