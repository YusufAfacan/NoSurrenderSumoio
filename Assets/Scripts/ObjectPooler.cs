using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private float spawnRadius;

    public static ObjectPooler Instance;

    public event EventHandler OnObjectSpawned;

    private Timer timer;

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
        public float spawnTime;
        public float spawnInterval;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = Timer.Instance;

        InitiliazePools();
    }

    private void InitiliazePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // what amount of object going to be needed during game is calculated
            // and instantiated beforehand

            if (pool.spawnInterval > 0)
            {
                pool.size = Mathf.CeilToInt(timer.gameTime / pool.spawnInterval);
            }

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(pool.parent);
            }

            poolDictionary.Add(pool.tag, objectPool);


            // if object has an spawn interval, spawning of object in that interval initiated

            if (pool.spawnInterval > 0)
            {
                StartCoroutine(StartSpawningDuringGame(pool.spawnTime, pool.spawnInterval, pool.tag));
            }
        }
    }

    private IEnumerator StartSpawningDuringGame(float startTime, float interval, string tag)
    {
        yield return new WaitForSeconds(startTime);

        while (true)
        {
            GameObject prefab = poolDictionary[tag].Dequeue();

            Vector3 spawnPosition;

            // random xPos of spawnPosition
            float xPos = Random.Range(-spawnRadius, spawnRadius);

            // zPosition of spawnPosition limited so position stays in radius as game area is circular
            float zBorder = Mathf.Sqrt(Mathf.Pow(spawnRadius,2) - Mathf.Pow(xPos,2));
            float zPos = Random.Range(-zBorder, zBorder);

            spawnPosition = new Vector3(xPos, 0.25f, zPos);

            prefab.SetActive(true);
            prefab.transform.position = spawnPosition;
            prefab.transform.DOShakePosition(0.5f, 0.5f);
            prefab.transform.DOShakeRotation(0.5f, 0.5f);
            prefab.transform.DOShakeScale(0.5f, 0.5f);

            poolDictionary[tag].Enqueue(prefab);

            OnObjectSpawned?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(interval);
        }
    }

    public GameObject SpawnFromObjectPooler(string tag)
    {
        GameObject prefab = poolDictionary[tag].Dequeue();

        prefab.SetActive(true);

        poolDictionary[tag].Enqueue(prefab);

        return prefab;
    }
}
