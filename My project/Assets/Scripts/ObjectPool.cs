using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Pool Settings")]
    [SerializeField] private List<GameObject> projectilePrefabs;
    [SerializeField] private int amountOfEachPrefab = 15;

    private Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        foreach (GameObject prefab in projectilePrefabs)
        {
            string key = prefab.name;
            if (!pools.ContainsKey(key))
            {
                pools.Add(key, new List<GameObject>());

                for (int i = 0; i < amountOfEachPrefab; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.name = key;
                    obj.SetActive(false);
                    pools[key].Add(obj);
                }
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        string key = prefab.name;

        if (pools.ContainsKey(key))
        {
            foreach (GameObject obj in pools[key])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
        }

        Debug.LogWarning($"Pula dla {key} jest pusta! Zwiêksz amountOfEachPrefab.");
        return null;
    }
}