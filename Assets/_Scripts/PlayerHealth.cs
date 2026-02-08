using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHP = 7;
    [SerializeField] private int currentHP;

    [Header("Invincibility")]
    [SerializeField] private float invincibleTime = 0.6f;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;
    public float Normalized => maxHP <= 0 ? 0f : (float)currentHP / maxHP;
    public bool IsDead => currentHP <= 0;
    public bool IsInvincible => _invincibleTimer > 0f;

    public event Action<int, int> OnHpChanged; // (current, max)
    public event Action OnDamaged;
    public event Action OnDead;

    float _invincibleTimer;

    void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (currentHP == 0) currentHP = maxHP;
        OnHpChanged?.Invoke(currentHP, maxHP);
    }

    void Update()
    {
        if (_invincibleTimer > 0f)
            _invincibleTimer -= Time.deltaTime;
    }

    public void ResetHP()
    {
        currentHP = maxHP;
        _invincibleTimer = 0f;
        OnHpChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (IsDead) return;
        if (IsInvincible) return;

        currentHP = Mathf.Max(0, currentHP - amount);
        _invincibleTimer = invincibleTime;

        OnDamaged?.Invoke();
        OnHpChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
            OnDead?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (IsDead) return;

        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHpChanged?.Invoke(currentHP, maxHP);
    }
}
