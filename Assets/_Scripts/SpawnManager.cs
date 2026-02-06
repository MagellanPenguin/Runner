using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Positions")]
    public Transform obstacleSpawnPoint;
    public Transform coinSpawnPoint;

    [Header("Pool Keys (registered in SimplePool)")]
    public string[] obstacleKeys;
    public string[] coinKeys;

    [Header("Spawn Timing")]
    public float obstacleInterval = 1.2f;
    public float coinInterval = 0.6f;
    float obstacleT, coinT;

    [Header("Y Ranges")]
    public Vector2 obstacleYRange = new Vector2(-2f, -2f); // 고정이면 min=max
    public Vector2 coinYRange = new Vector2(-1f, 2f);

    void Update()
    {
        obstacleT += Time.deltaTime;
        coinT += Time.deltaTime;

        if (obstacleKeys.Length > 0 && obstacleT >= obstacleInterval)
        {
            obstacleT = 0f;
            SpawnRandom(obstacleKeys, obstacleSpawnPoint.position, obstacleYRange);
        }

        if (coinKeys.Length > 0 && coinT >= coinInterval)
        {
            coinT = 0f;
            // 코인은 확률로 스폰해도 됨
            if (Random.value < 0.7f)
                SpawnRandom(coinKeys, coinSpawnPoint.position, coinYRange);
        }
    }

    void SpawnRandom(string[] keys, Vector3 basePos, Vector2 yRange)
    {
        var key = keys[Random.Range(0, keys.Length)];
        float y = Random.Range(yRange.x, yRange.y);
        var pos = new Vector3(basePos.x, y, basePos.z);
        SimplePool.Instance.Spawn(key, pos, Quaternion.identity);
    }
}
