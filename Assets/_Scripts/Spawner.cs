using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;

    public float spawnX = 12f;
    public float minY = -2f;
    public float maxY = 2f;

    public float interval = 1.2f;
    float t;

    void Update()
    {
        t += Time.deltaTime;
        if (t < interval) return;
        t = 0;

        var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(prefab, new Vector3(spawnX,Random.Range(minY,maxY), 0), Quaternion.identity);
    }
}
