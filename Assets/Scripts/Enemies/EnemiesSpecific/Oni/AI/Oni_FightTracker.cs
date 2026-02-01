using System.IO;
using System.Text;
using UnityEngine;

public class Oni_FightTracker : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Oni oni;
    [SerializeField] private Boss_AIBrain boss_AI;

    private Transform player;
    //[SerializeField] private Transform player;
    

    public string mobName = "Oni";
    public float maxFightDuration = 10f;
    public bool mobFightActive = false;

    public float fightAmount;
    public float allFightDuration;

    public float averageFightDuration;
    public float durationOfMobFight;
    public float playerEndHP;

    private float fightStartTime;
    private float playerStartHP;
    //private float playerDistance;

    public int mobMeleeAttacks;
    public int mobRangeAttacks;

    private int startMobMeleeAttacks;
    private int startMobRangeAttacks;

    private string logPath;

    void Start()
    {
        logPath = Path.Combine(Application.persistentDataPath, "mob_fight_log.csv");

        if (!File.Exists(logPath))
        {
            File.WriteAllText(logPath, "mobName,startHP,endHP,fightDuration,playerDistance\n");
        }

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            Debug.Log("assigned player go");
        }
    }

    void Update()
    {
        if (mobFightActive && Time.time - fightStartTime >= maxFightDuration)
        {
            Debug.Log($"{mobName} fight max duration reached. Forcing end.");
            EndFight();
        }

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
            Debug.Log("Player cannot be found");
        }
        //Debug.Log(player.position.x);
        //Debug.Log(oni.CurrentPosX);
    }

    public void StartFight()
    {
        if (mobFightActive) return;

        fightStartTime = Time.time;

        startMobMeleeAttacks = boss_AI.meleeAttacks;
        startMobRangeAttacks = boss_AI.rangeAttacks;

        playerStartHP = playerStats.GetHPPercent();
        mobFightActive = true;
        Debug.Log("Fight started");
    }

    public void EndFight()
    {
        if (!mobFightActive) return;

        float fightEndTime = Time.time;
        durationOfMobFight = Mathf.Min(fightEndTime - fightStartTime, maxFightDuration);

        fightAmount++;
        allFightDuration += durationOfMobFight;
        averageFightDuration = allFightDuration / fightAmount;

        mobMeleeAttacks = (boss_AI.meleeAttacks - startMobMeleeAttacks);
        mobRangeAttacks = (boss_AI.rangeAttacks - startMobRangeAttacks);

        playerEndHP = playerStats.GetHPPercent();

        LogFight(mobName, playerStartHP, playerEndHP, durationOfMobFight, mobMeleeAttacks, mobRangeAttacks);

        mobFightActive = false;

        Debug.Log("Fight ended");
    }

    void LogFight(string mob, float startHP, float endHP, float duration, int meleeAttacks, int rangeAttacks)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(mob).Append(",");
        sb.Append(startHP.ToString("F2")).Append(",");
        sb.Append(endHP.ToString("F2")).Append(",");
        sb.Append(duration.ToString("F2")).Append(",");
        sb.Append(meleeAttacks.ToString("F2")).Append(",");
        sb.Append(rangeAttacks.ToString("F2")).Append("\n");

        File.AppendAllText(logPath, sb.ToString());

        Debug.Log($"Fight logged: {mob}, duration {duration:F2}s, fight amount {fightAmount}, melee hits {mobMeleeAttacks}, range hits {mobRangeAttacks}");
    }

    public void ForceEndFight()
    {
        if (!mobFightActive) return;
        EndFight();
    }


    //Not ognna use this
    /*
    private float GetDistance()
    {
        float distanceX = Mathf.Abs(oni.CurrentPosX - player.position.x);
        return distanceX;
    }
    */
}
