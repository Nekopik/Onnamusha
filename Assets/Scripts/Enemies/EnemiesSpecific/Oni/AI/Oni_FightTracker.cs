using System.IO;
using System.Text;
using UnityEngine;

public class Oni_FightTracker : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;


    public string mobName = "Oni";
    public float maxFightDuration = 10f;
    public bool mobFightActive = false;

    public float fightAmount;
    public float allFightDuration;
    public float averageFightDuration;

    private float fightStartTime;
    private float playerStartHP;

    private string logPath;

    void Start()
    {
        logPath = Path.Combine(Application.persistentDataPath, "mob_fight_log.csv");

        if (!File.Exists(logPath))
        {
            File.WriteAllText(logPath, "mobName,startHP,endHP,fightDuration\n");
        }
    }

    void Update()
    {
        if (mobFightActive && Time.time - fightStartTime >= maxFightDuration)
        {
            Debug.Log($"{mobName} fight max duration reached. Forcing end.");
            EndFight();
        }
    }

    public void StartFight()
    {
        if (mobFightActive) return;

        fightStartTime = Time.time;

        playerStartHP = playerStats.GetHPPercent();
        mobFightActive = true;
        Debug.Log("Fight started");
    }

    public void EndFight()
    {
        if (!mobFightActive) return;

        float fightEndTime = Time.time;
        float duration = Mathf.Min(fightEndTime - fightStartTime, maxFightDuration);

        fightAmount++;
        allFightDuration += duration;
        averageFightDuration = allFightDuration / fightAmount;


        float playerEndHP = playerStats.GetHPPercent();

        LogFight(mobName, playerStartHP, playerEndHP, duration);

        mobFightActive = false;

        Debug.Log("Fight ended");
    }

    void LogFight(string mob, float startHP, float endHP, float duration)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(mob).Append(",");
        sb.Append(startHP.ToString("F2")).Append(",");
        sb.Append(endHP.ToString("F2")).Append(",");
        sb.Append(duration.ToString("F2")).Append("\n");

        File.AppendAllText(logPath, sb.ToString());

        Debug.Log($"Fight logged: {mob}, duration {duration:F2}s, fight amount {fightAmount}");
    }

    public void ForceEndFight()
    {
        if (!mobFightActive) return;
        EndFight();
    }
}
