using UnityEngine;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform respawnBossPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall;
    [SerializeField] private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;
    private bool visitCherryTree = false;

    private CinemachineCamera CVC;
    private Boss boss;

    private void Start()
    {
        CVC = GameObject.Find("PlayerCamera").GetComponent<CinemachineCamera>();
        wall.SetActive(false);
    }

    private void Update()
    {
        CheckRespawn();

    }

    public void Respawn()
    {
        if(!respawn)
        {
            respawnTimeStart = Time.time;
            respawn = true;
        }
        
    }

    private void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn && !visitCherryTree)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.Follow = playerTemp.transform;
            boss.SetBossFightInactive();
            wall.SetActive(false);
            respawn = false;
        }
        else if (Time.time >= respawnTimeStart + respawnTime && respawn && visitCherryTree)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.Follow = playerTemp.transform;
            boss.SetBossFightInactive();
            wall.SetActive(false);
            respawn = false;
        }
    }

    public void SetBossPlayerSpawnPoint()
    {
        visitCherryTree = true;
    }

}
