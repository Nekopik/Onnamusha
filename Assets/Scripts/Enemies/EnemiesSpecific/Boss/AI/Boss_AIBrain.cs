using Unity.Barracuda;
using UnityEngine;

public class Boss_AIBrain : MonoBehaviour
{
    [SerializeField] private NNModel bossModeModel;
    private Model runtimeModel;
    private IWorker worker;

    [SerializeField] private Boss boss;

    public int meleeAttacks;
    public int rangeAttacks;

    public float fightDuration;
    public float playerHpLoss;

    public float skillModifier = 1f;

    private bool bossFightStarted = false;

    void Start()
    {
        runtimeModel = ModelLoader.Load(bossModeModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
    }

    public void StartBossFight()
    {
        bossFightStarted = true;
        DecideSkillModifier();
    }

    public void DecideSkillModifier()
    {
        float total = meleeAttacks + rangeAttacks + 0.001f;

        float meleeRatio = meleeAttacks / total;
        float rangeRatio = rangeAttacks / total;

        float normalizedDuration = fightDuration / 10f;

        Tensor input = new Tensor(1, 4);

        input[0] = meleeRatio;
        input[1] = rangeRatio;
        input[2] = normalizedDuration;
        input[3] = playerHpLoss;

        worker.Execute(input);

        Tensor output = worker.PeekOutput();

        skillModifier = output[0];

        ApplyModifier(skillModifier);

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

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
