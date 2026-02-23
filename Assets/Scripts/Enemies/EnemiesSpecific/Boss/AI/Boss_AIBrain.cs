using System.Globalization;
using System.Text;
using Unity.Barracuda;
using UnityEngine;
using System.IO;

public class Boss_AIBrain : MonoBehaviour
{
    [SerializeField] private NNModel bossModeModel;
    private Model runtimeModel;
    private IWorker worker;

    [SerializeField] private Boss boss;

    private Oni_FightTracker oniTracker;

    public int meleeAttacks;
    public int rangeAttacks;

    public float fightDuration;
    public float playerHpLoss;

    public int arrivedAtBoss = 0;

    public float skillModifier = 1f;

    private bool bossFightStarted = false;

    private string logPath;

    void Start()
    {
        logPath = Path.Combine(Application.persistentDataPath, "skill_modifier_data.csv");

        runtimeModel = ModelLoader.Load(bossModeModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
    }

    private void Awake()
    {
        if (oniTracker == null)
            oniTracker = FindFirstObjectByType<Oni_FightTracker>();
    }

    public void StartBossFight()
    {
        bossFightStarted = true;
        arrivedAtBoss = 1;
        DecideSkillModifier();
    }

    public void DecideSkillModifier()
    {
        float total = meleeAttacks + rangeAttacks + 0.001f;

        float meleeRatio = meleeAttacks / total;
        float rangeRatio = rangeAttacks / total;

        float normalizedDuration = fightDuration / 10f;

        Tensor input = new Tensor(1, 5);

        input[0] = meleeRatio;
        input[1] = rangeRatio;
        input[2] = normalizedDuration;
        input[3] = playerHpLoss;
        input[4] = arrivedAtBoss;

        worker.Execute(input);

        Tensor output = worker.PeekOutput();

        skillModifier = output[0];

        LogDataToSkillModifier(meleeRatio, rangeRatio, normalizedDuration, playerHpLoss, arrivedAtBoss);
        ApplyModifier(skillModifier);

        Debug.Log("melee ratio: " + meleeRatio + " range ratio: " + rangeRatio + " duration: " + normalizedDuration + " player HP loss: " + playerHpLoss);
        Debug.Log("Boss Skill Modifier: " + skillModifier.ToString("F2"));

        input.Dispose();
        output.Dispose();
    }

    private void ApplyModifier(float modifier)
    {
        modifier = Mathf.Clamp(modifier, 0.8f, 2f);

        boss.ApplyAIModifier(skillModifier);
    }

    public void RegisterMeleeAttack()
    {
        meleeAttacks++;
        Debug.Log("Melee attack done");
    }

    public void RegisterRangeAttack()
    {
        rangeAttacks++;
        Debug.Log("Range attack done");
    }

    void LogDataToSkillModifier(float logMeleeRatio, float logRangeRatio, float logNormalizedDuration, float logPlayerHPLoss, int logArrivedAtBoss)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(logMeleeRatio.ToString("F2", CultureInfo.InvariantCulture)).Append(",");
        sb.Append(logRangeRatio.ToString("F2", CultureInfo.InvariantCulture)).Append(",");
        sb.Append(logNormalizedDuration.ToString("F2", CultureInfo.InvariantCulture)).Append(",");
        sb.Append(logPlayerHPLoss.ToString("F2", CultureInfo.InvariantCulture)).Append(",");
        sb.Append(logArrivedAtBoss.ToString("F2", CultureInfo.InvariantCulture)).Append("\n");

        File.AppendAllText(logPath, sb.ToString());
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
