using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [Header("UI")]
    public Image hpFill;          
    public Image flameFill;       
    public float flameBoost = 0.03f;

    [Header("Target")]
    public PlayerHealth target;

    void Awake()
    {
        if (!hpFill) Debug.LogWarning("hpFill 확인되지않음");
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
