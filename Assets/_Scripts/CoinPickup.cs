using UnityEngine;
using UnityEngine.Events;

public class CoinPickup : MonoBehaviour
{
    public int amount = 1;
    public UnityEvent<int> onCollected;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        onCollected?.Invoke(amount);
        // 예: CoinManager.Instance.Add(amount);

        // 먹으면 바로 풀로 반환
        SimplePool.Instance?.Despawn(gameObject);
    }
}
