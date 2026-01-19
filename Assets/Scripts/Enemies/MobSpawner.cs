using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private Transform[] spawnPoints;

    void Awake()
    {
        FindSpawnPoints();
    }

    void Start()
    {
        SpawnAll();
    }

    void FindSpawnPoints()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[points.Length];

        for (int i = 0; i < points.Length; i++)
            spawnPoints[i] = points[i].transform;
    }

    void SpawnAll()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(mobPrefab, point.position, Quaternion.identity);
        }
    }
}
