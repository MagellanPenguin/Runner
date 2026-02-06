using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 12f;
    public float slideDuration = 0.6f;

    [Header("HP")]
    public int maxHP = 7;
    public int currentHP;

    [Header("References")]
    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public Collider2D headCollider;
    public Collider2D footCollider;

    bool isGrounded;
    bool isSliding;

    void Awake()
    {
        currentHP = maxHP;
    }

    /* =======================
     * INPUT (Unity Events)
     * ======================= */

    public void OnJump()
    {
        if (!isGrounded || isSliding) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    public void OnSlideStart()
    {
        if (!isGrounded || isSliding) return;

        isSliding = true;

        // 슬라이드 중 머리 충돌 비활성화 (선택)
        headCollider.enabled = false;

        Invoke(nameof(EndSlide), slideDuration);
    }

    void EndSlide()
    {
        isSliding = false;
        headCollider.enabled = true;
    }

    /* =======================
     * GROUND CHECK
     * ======================= */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && other == footCollider)
        {
            isGrounded = true;
        }
    }

    /* =======================
     * DAMAGE
     * ======================= */

    public void TakeDamage(int amount)
    {
        if (currentHP <= 0) return;

        currentHP -= amount;
        Debug.Log($"HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        // Game Over 처리
    }
}
