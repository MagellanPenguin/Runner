using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Tooltip("추가 가속/감속 배율(코인 느리게 등)")]
    public float speedMultiplier = 1f;

    void Update()
    {
        float s = RunSpeedController.Instance ? RunSpeedController.Instance.CurrentSpeed : 0f;
        transform.position += Vector3.left * (s * speedMultiplier) * Time.deltaTime;
    }
}
