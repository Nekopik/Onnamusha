using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private Transform spawnPointContainer;
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        if (spawnPointContainer == null) return;

        foreach (Transform point in spawnPointContainer)
        {
            Instantiate(mobPrefab, point.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        // If you haven't assigned the container yet, try to find it among children
        if (spawnPointContainer == null)
            spawnPointContainer = transform.Find("SpawnPoints");

        if (spawnPointContainer == null) return;

        Gizmos.color = Color.cyan;
        foreach (Transform point in spawnPointContainer)
        {
            // Draw a diamond at each point inside the container
            Gizmos.DrawWireCube(point.position, Vector3.one * 0.3f);

            // Draw a line from the spawner to the point
            Gizmos.DrawLine(transform.position, point.position);
        }
    }
}