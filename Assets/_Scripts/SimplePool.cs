using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    public static SimplePool Instance { get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public string key;
        public GameObject prefab;
        public int prewarm = 10;
    }

    public List<PoolEntry> entries = new();
    Dictionary<string, Queue<GameObject>> pools = new();
    Dictionary<GameObject, string> prefabKeyByInstance = new();

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var e in entries)
        {
            if (!pools.ContainsKey(e.key)) pools[e.key] = new Queue<GameObject>();

            for (int i = 0; i < e.prewarm; i++)
            {
                var go = Instantiate(e.prefab, transform);
                go.SetActive(false);
                pools[e.key].Enqueue(go);
                prefabKeyByInstance[go] = e.key;
            }
        }
    }

    public GameObject Spawn(string key, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(key, out var q))
        {
            Debug.LogError($"Pool key not found: {key}");
            return null;
        }

        GameObject go = q.Count > 0 ? q.Dequeue() : Instantiate(GetPrefab(key), transform);
        prefabKeyByInstance[go] = key;

        go.transform.SetPositionAndRotation(pos, rot);
        go.SetActive(true);
        return go;
    }

    public void Despawn(GameObject go)
    {
        if (!go) return;

        if (!prefabKeyByInstance.TryGetValue(go, out var key))
        {
            // 풀에서 생성되지 않았으면 그냥 파괴
            Destroy(go);
            return;
        }

        go.SetActive(false);
        go.transform.SetParent(transform, true);
        pools[key].Enqueue(go);
    }

    GameObject GetPrefab(string key)
    {
        foreach (var e in entries)
            if (e.key == key) return e.prefab;
        return null;
    }
}
