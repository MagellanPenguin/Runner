using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("Refs")]
    public PlayerHealth health;

    [Header("Damage Filter")]
    public string groundTag = "Ground";
    public LayerMask damageLayers; // Obstacle 레이어만 체크 추천

    [Header("Damage")]
    public int defaultDamage = 1;

    void Reset()
    {
        health = GetComponent<PlayerHealth>();
        // Inspector에서 Obstacle 레이어만 체크해두면 됨
    }

    bool IsDamageTarget(Collider2D other)
    {
        // 1) Ground 태그면 절대 데미지 X
        if (other.CompareTag(groundTag)) return false;

        // 2) 레이어가 damageLayers에 포함되면 데미지 O
        int otherLayerMask = 1 << other.gameObject.layer;
        return (damageLayers.value & otherLayerMask) != 0;
    }

    void ApplyDamageFrom(Collider2D other)
    {
        if (!health || health.IsDead) return;
        if (!IsDamageTarget(other)) return;

        // 장애물 쪽에서 데미지 값을 주고 싶으면 ObstacleDamageSource로 확장 가능
        health.TakeDamage(defaultDamage);
    }

    // 몸통 충돌(Non-trigger)
    void OnCollisionEnter2D(Collision2D col)
    {
        ApplyDamageFrom(col.collider);
    }

    // 머리/발 Trigger 충돌 등
    void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamageFrom(other);
    }
}
