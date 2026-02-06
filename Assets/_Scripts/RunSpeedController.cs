using UnityEngine;

public class RunSpeedController : MonoBehaviour
{
    public static RunSpeedController Instance { get; private set; }

    [Header("Speed")]
    public float startSpeed = 4f;
    public float maxSpeed = 10f;

    [Header("Ramp (seconds)")]
    public float rampDuration = 120f; // 2 minutes

    [Header("Optional curve (0..1 input => 0..1 output)")]
    public AnimationCurve rampCurve = AnimationCurve.Linear(0, 0, 1, 1);

    float elapsed;

    public float CurrentSpeed { get; private set; }

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        CurrentSpeed = startSpeed;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        float t = rampDuration <= 0f ? 1f : Mathf.Clamp01(elapsed / rampDuration);
        float eased = rampCurve.Evaluate(t);              // 0..1
        CurrentSpeed = Mathf.Lerp(startSpeed, maxSpeed, eased);
    }

    public void ResetRun()
    {
        elapsed = 0f;
        CurrentSpeed = startSpeed;
    }
}
