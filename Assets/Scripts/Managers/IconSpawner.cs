using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class IconSpawner : MonoBehaviour
{
    public ObjectPool IconPool { get; private set; }
    public Grid Grid { get; private set; }

    [SerializeField] private float xMinBounds;
    [SerializeField] private float xMaxBounds;
    [SerializeField] private float yMinBounds;
    [SerializeField] private float yMaxBounds;
    private Vector3 pickedSpawnPosition = Vector3.zero;

    public float TimePassed { get; private set; } = 0.0f;
    float timeSinceLastSpawn = 0.0f;
    float timeTillRateUp = 10.0f;
    float spawnRate = 5f;

    bool searching = false;

    // Use this for initialization
    void Start()
    {
        IconPool = GetComponent<ObjectPool>();
        Grid = FindObjectOfType<Grid>();
        if (Grid == null) throw new System.ArgumentException("Grid not found, please make sure there is a grid in the scene.");
    }

    // Update is called once per frame
    void Update()
    {
        if (!RecycleBin.GameOver)
        {
            TimePassed += Time.deltaTime;
            timeSinceLastSpawn += Time.deltaTime;
            timeTillRateUp -= Time.deltaTime;

            if (timeSinceLastSpawn >= spawnRate)
            {
                SpawnIcon();
                timeSinceLastSpawn = 0;
            }

            if (timeTillRateUp <= 0.0f || ((RecycleBin.score % 5 == 0) && RecycleBin.score != 0))
            {
                spawnRate -= 0.5f;
                if (spawnRate <= 1.0f)
                {
                    spawnRate = TimePassed > 50.0f ? 0.5f : 1.0f;
                }
                timeTillRateUp = 10.0f;
            }
        }
        else
        {
            StopAllCoroutines();
            searching = false;
            pickedSpawnPosition = Vector3.zero;
        }
    }

    void SpawnIcon()
    {
        if (!searching && IconPool.PoolAvailable())
        {
            StartCoroutine(FindSpawnPoint());
        }

        if (pickedSpawnPosition != Vector3.zero && IconPool.PoolAvailable())
        {
            GameObject icon = IconPool.GetAvailableObject();
            icon.GetComponent<InteractableIcon>().Reset();

            icon.transform.position = pickedSpawnPosition;

            IconSnapToGrid iconSnap = icon.GetComponent<IconSnapToGrid>();
            iconSnap.Init();
            if (!iconSnap.TrySnapIcon())
            {
                IconPool.ReturnGameObjectToPool(iconSnap.gameObject);
            }

            icon = null;
            StopAllCoroutines();
            searching = false;
            pickedSpawnPosition = Vector3.zero;
        }
    }

    IEnumerator FindSpawnPoint ()
    {
        searching = true;
        bool spawnPointFound = false;
        float spawnX = 0;
        float spawnY = 0;

        do
        {
            spawnX = Random.Range(xMinBounds, xMaxBounds);
            spawnY = Random.Range(yMinBounds, yMaxBounds);

            if(Grid.CheckSpotAvailability(new Vector3(spawnX, spawnY, 1)))
            {
                pickedSpawnPosition = new Vector3(spawnX, spawnY, 1);
                spawnPointFound = true;
                searching = false;
            }
            else
            {
                yield return null;
            }
        }
        while (!spawnPointFound || !RecycleBin.GameOver);
    }

    public void Reset()
    {
        timeTillRateUp = 10.0f;
        timeSinceLastSpawn = 0.0f;
        TimePassed = 0.0f;

        searching = false;
        spawnRate = 5f;
        pickedSpawnPosition = Vector3.zero;

        IconPool.ResetPool();

        for (int i = 0; i < 5; i++)
        {
            SpawnIcon();
        }
    }
}
