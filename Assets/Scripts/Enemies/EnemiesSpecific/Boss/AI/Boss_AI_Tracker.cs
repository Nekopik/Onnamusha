using System.IO;
using System.Text;
using UnityEngine;

public class Boss_AI_Tracker : MonoBehaviour
{
    [SerializeField] private Boss_AIBrain boss_AI;
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BossFightStartTrigger bossFightStartTrigger;

    private float logStartHP;
    private float logEndHP;
    private float logDuration;
    private float logMeleeAttacks;
    private float logRangeAttacks;
    private float logMeleePreference;

    private string logPath;

    private void Start()
    {
        logPath = Path.Combine(Application.persistentDataPath, "boss_fight_log.csv");

        if (!File.Exists(logPath))
        {
            File.WriteAllText(logPath, "startHP,endHP,fightDuration,meleeAttacks,rangeAttacks,meleePreference\n");
        }
    }

    public void LogBossFight()
    {
        logStartHP = bossFightStartTrigger.PlayerBossFightStartHP;
        logEndHP = playerStats.GetHPPercent();
        if ((boss.bossEndTime - boss.bossStartTime < 0))
        {
            logDuration = 0;
        }
        else
        {
            logDuration = boss.bossEndTime - boss.bossStartTime;
        }
        logMeleeAttacks = boss_AI.meleeAttacks;
        logRangeAttacks = boss_AI.rangeAttacks;
        logMeleePreference = boss_AI.meleePreference;

        LogBossFightBuilder(logStartHP, logEndHP, logDuration, logMeleeAttacks, logRangeAttacks, logMeleePreference);

        Debug.Log("Boss fight logged");
    }

    void LogBossFightBuilder(float startHP, float endHP, float duration, float meleeAttacks, float rangeAttacks, float meleePreference)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(startHP).Append(",");
        sb.Append(endHP.ToString("F2")).Append(",");
        sb.Append(duration.ToString("F2")).Append(",");
        sb.Append(meleeAttacks.ToString("F2")).Append(",");
        sb.Append(rangeAttacks.ToString("F2")).Append(",");
        sb.Append(meleePreference.ToString("F2")).Append("\n");
    }

    private void Update()
    {
        if (playerStats == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerStats = player.GetComponent<PlayerStats>();
            }
            return;
        }
    }
}
