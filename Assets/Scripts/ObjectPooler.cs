using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public event EventHandler OnHealthKitSpawned;
    private Timer timer;


    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
        public int spawnTime;
        public int spawnInterval;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;


    private void Awake()
    {
        Instance = this;
        InitiliazePools();
        
        //InvokeRepeating(nameof(SpawnHealthKit), 5, 2);
    }

    private void Start()
    {
        timer = Timer.Instance;
    }

    private void InitiliazePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // what amount of object going to be needed during game is calculated
            // and instantiated beforehand
            pool.size = Mathf.CeilToInt(timer.gameTime / pool.spawnInterval);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(pool.parent);
            }

            poolDictionary.Add(pool.tag, objectPool);

            StartCoroutine(StartSpawningDuringGame(pool.prefab, pool.spawnTime, pool.spawnInterval, pool.tag));

        }
    }

    private IEnumerator StartSpawningDuringGame(GameObject prefab, float startTime, float interval, string tag)
    {
        yield return new WaitForSeconds(startTime);


        while (true)
        {

            float xPos = Random.Range(-5f, 5f);
            float zPos = Random.Range(-4f, 4f);

            prefab.SetActive(true);
            prefab.transform.position = new Vector3(xPos, 0.25f, zPos);
            prefab.transform.DOShakePosition(0.5f, 0.5f);
            prefab.transform.DOShakeRotation(0.5f, 0.5f);
            prefab.transform.DOShakeScale(0.5f, 0.5f);

            poolDictionary[tag].Enqueue(prefab);


            yield return new WaitForSeconds(interval);
        }

    }



    private void SpawnHealthKit()
    {
        GameObject healthKit = poolDictionary["HealthKit"].Dequeue();

        float xPos = Random.Range(-5f, 5f);
        float zPos = Random.Range(-4f, 4f);

        healthKit.SetActive(true);
        healthKit.transform.position = new Vector3(xPos, 0.25f, zPos);
        healthKit.transform.DOShakePosition(0.5f, 0.5f);
        healthKit.transform.DOShakeRotation(0.5f, 0.5f);
        healthKit.transform.DOShakeScale(0.5f, 0.5f);


        poolDictionary["HealthKit"].Enqueue(healthKit);
        OnHealthKitSpawned?.Invoke(this, EventArgs.Empty);
    }
}
