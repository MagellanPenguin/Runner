using UnityEngine;

/// <summary>
/// 플레이어가 충돌했을 때
/// - Ground 레이어면 무시
/// - Wall 레이어면 데미지 적용
/// </summary>
public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("Refs")]
    public PlayerHealth health;

    [Header("Layer Filters")]
    [Tooltip("데미지를 받지 않는 레이어 (Ground)")]
    public LayerMask groundLayer;

    [Tooltip("데미지를 받는 레이어 (Wall)")]
    public LayerMask wallLayer;

    [Header("Damage")]
    public int defaultDamage = 1;

    void Reset()
    {
        health = GetComponent<PlayerHealth>();
    }

    bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    bool ShouldTakeDamage(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        // 1) Ground면 절대 데미지 X
        if (IsInLayerMask(otherLayer, groundLayer))
            return false;

        // 2) Wall이면 데미지 O
        if (IsInLayerMask(otherLayer, wallLayer))
            return true;

        // 3) 그 외 레이어는 무시
        return false;
    }

    void ApplyDamage(Collider2D other)
    {
        if (!health || health.IsDead) return;
        if (!ShouldTakeDamage(other)) return;

        health.TakeDamage(defaultDamage);
    }

    // 몸통 충돌
    void OnCollisionEnter2D(Collision2D col)
    {
        ApplyDamage(col.collider);
    }

    // 머리 / 발 Trigger 충돌
    void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamage(other);
    }
}
