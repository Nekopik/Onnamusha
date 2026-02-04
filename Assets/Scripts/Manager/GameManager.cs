using UnityEngine;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform respawnBossPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall;
    [SerializeField] private Boss boss;
    [SerializeField] private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;
    private bool visitCherryTree = false;

    private CinemachineCamera CVC;
    

    private void Start()
    {
        GameObject scenePlayer = GameObject.FindGameObjectWithTag("Player");

        if (boss == null)
        {
            boss = GameObject.Find("Boss").GetComponent<Boss>();
        }

        if (scenePlayer != null)
        {
            player = scenePlayer;
        }
        else
        {
            Debug.LogError("GameManager: No object tagged 'Player' found in the scene!");
        }

        CVC = GameObject.Find("PlayerCamera").GetComponent<CinemachineCamera>();
        wall.SetActive(false);
        boss.SetBossFightInactive();
    }

    private void Update()
    {
        CheckRespawn();

    }


    public void Respawn()
    {
        if (!respawn)
        {
            respawnTimeStart = Time.time;
            respawn = true;
            boss.SetBossFightInactive();
            Debug.Log("Respawn Timer Started");
        }
    }

    private void CheckRespawn()
    {
        if (respawn && Time.time >= respawnTimeStart + respawnTime)
        {
            // FORCE Unity to find the object in the scene if the current reference is a prefab
            if (player != null && !player.scene.IsValid())
            {
                // This happens if 'player' is a prefab asset. We need the one in the world.
                player = GameObject.FindGameObjectWithTag("Player");
            }

            if (player != null)
            {
                player.transform.position = GetRespawnPoint();

                PlayerStats stats = player.GetComponent<PlayerStats>();
                if (stats != null) stats.ResetStats();

                // Reactivate the hierarchy instance
                player.SetActive(true);

                if (CVC != null) CVC.Follow = player.transform;

                respawn = false;
                Debug.Log("ACTUAL Scene Player reactivated at " + player.transform.position);
            }
            else
            {
                Debug.LogError("GameManager cannot find a Player instance in the scene to reactivate!");
            }
        }
    }

    public Vector3 GetRespawnPoint()
    {
        return visitCherryTree ? respawnBossPoint.position : respawnPoint.position;
    }

    /*
    private void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn && !visitCherryTree)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.Follow = playerTemp.transform;
            respawn = false;
        }
        else if (Time.time >= respawnTimeStart + respawnTime && respawn && visitCherryTree)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.Follow = playerTemp.transform;
            respawn = false;
        }
    }
    */

    public void SetBossPlayerSpawnPoint()
    {
        visitCherryTree = true;
        Debug.Log("Cherry tree visited");
    }

}
