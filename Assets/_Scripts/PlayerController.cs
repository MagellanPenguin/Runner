using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 12f;
    public float slideDuration = 0.6f;

    [Header("References")]
    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public Collider2D headCollider;
    public Collider2D footCollider;

    [Header("Systems")]
    public PlayerHealth health;

    [Header("Animation (ALL TRIGGERS)")]
    public Animator animator;
    public string trigMove = "Move";
    public string trigJump = "Jump";
    public string trigSliding = "Sliding";
    public string trigHit = "Hit";
    public string trigDie = "Die";

    bool isGrounded;
    bool isSliding;
    float slideTimer;

    // 러너는 항상 달리니까 Move 트리거는 "한 번만" 쏘는게 일반적
    bool moveTriggeredOnce;

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<PlayerHealth>();
    }

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!health) health = GetComponent<PlayerHealth>();
    }

    void OnEnable()
    {
        if (health != null)
        {
            health.OnDamaged += HandleDamaged;
            health.OnDead += HandleDead;
        }
    }

    void OnDisable()
    {
        if (health != null)
        {
            health.OnDamaged -= HandleDamaged;
            health.OnDead -= HandleDead;
        }
    }

    void Start()
    {
        // 자동 달리기 시작 애니(원한다면)
        FireMoveTriggerOnce();
    }

    void Update()
    {
        if (health != null && health.IsDead) return;

        // 슬라이드 타이머
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                EndSlide();
        }
    }

    /* =======================
     * INPUT (PlayerInput - Unity Events에서 연결)
     * ======================= */

    // Gameplay/Jump Performed
    public void OnJump()
    {
        if (health != null && health.IsDead) return;
        if (!isGrounded || isSliding) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;

        // Jump 트리거
        FireTrigger(trigJump);
    }

    // Gameplay/Slide Started(또는 Performed)
    public void OnSlideStart()
    {
        if (health != null && health.IsDead) return;
        if (!isGrounded || isSliding) return;

        isSliding = true;
        slideTimer = slideDuration;

        // 슬라이드 중 머리 충돌 비활성화(원하는 경우)
        if (headCollider) headCollider.enabled = false;

        // Sliding 트리거
        FireTrigger(trigSliding);
    }

    // Hold 액션이면 Gameplay/Slide Canceled에 연결(선택)
    public void OnSlideEndInput()
    {
        if (!isSliding) return;
        EndSlide();
    }

    void EndSlide()
    {
        isSliding = false;

        if (headCollider) headCollider.enabled = true;

        // 슬라이드 끝나면 다시 달리기 트리거를 쏘고 싶으면(선택)
        // FireTrigger(trigMove);
        // 다만 Move가 트리거라면 너무 자주 쏘는 건 비추.
        FireMoveTriggerOnce(); // "안 쏜 상태"라면 1회만
    }

    /* =======================
     * GROUND CHECK
     * ======================= */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;

            // 착지하면 달리기 모션이 필요한 경우(선택)
            FireMoveTriggerOnce();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = false;
    }

    /* =======================
     * HEALTH EVENTS -> ANIM
     * ======================= */

    void HandleDamaged()
    {
        FireTrigger(trigHit);
    }

    void HandleDead()
    {
        FireTrigger(trigDie);

        // 죽으면 입력/움직임 막고 싶으면 여기서 처리(선택)
        // rb.simulated = false;
        // GetComponent<UnityEngine.InputSystem.PlayerInput>()?.DeactivateInput();
    }

    /* =======================
     * ANIM HELPERS
     * ======================= */

    void FireTrigger(string trig)
    {
        if (!animator || string.IsNullOrEmpty(trig)) return;

        // 같은 프레임에 트리거가 꼬이는걸 줄이고 싶으면 Reset 후 Set
        animator.ResetTrigger(trig);
        animator.SetTrigger(trig);
    }

    void FireMoveTriggerOnce()
    {
        if (moveTriggeredOnce) return;
        moveTriggeredOnce = true;
        FireTrigger(trigMove);
    }

    // (선택) 리트라이/리스폰 시 호출
    public void ResetStateForRetry()
    {
        isGrounded = false;
        isSliding = false;
        slideTimer = 0f;
        moveTriggeredOnce = false;

        if (headCollider) headCollider.enabled = true;

        FireMoveTriggerOnce();
    }
}
