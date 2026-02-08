using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [Header("UI")]
    public Image hpFill;          // Filled Image
    public Image flameFill;       // (선택) 불꽃 오버레이도 Filled로 만들면 같이 줄어듦
    public float flameBoost = 0.03f;

    [Header("Target")]
    public PlayerHealth target;

    void Awake()
    {
        if (!hpFill) Debug.LogWarning("hpFill이 비어있음");
    }

    void OnEnable()
    {
        if (target != null)
            target.OnHpChanged += HandleHpChanged;
    }

    void OnDisable()
    {
        if (target != null)
            target.OnHpChanged -= HandleHpChanged;
    }

    void Start()
    {
        if (target != null)
            HandleHpChanged(target.CurrentHP, target.MaxHP);
    }

    void HandleHpChanged(int current, int max)
    {
        float t = max <= 0 ? 0f : (float)current / max;

        if (hpFill) hpFill.fillAmount = t;
        if (flameFill) flameFill.fillAmount = Mathf.Clamp01(t + flameBoost);
    }
}
