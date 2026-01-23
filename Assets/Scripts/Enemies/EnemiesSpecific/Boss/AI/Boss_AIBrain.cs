using Unity.Barracuda;
using UnityEngine;
using static Boss;

public class Boss_AIBrain : MonoBehaviour
{
    [SerializeField] private NNModel bossModeModel;

    private Model runtimeModel;
    private IWorker worker;

    [SerializeField] private Boss boss;

    public int meleeAttacks;
    public int rangeAttacks;

    [SerializeField] private float decisionInterval = 5f;
    private float decisionTimer;

    private bool bossFightStarted = false;

    void Start()
    {
        runtimeModel = ModelLoader.Load(bossModeModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
    }

    void Update()
    {
        if (!bossFightStarted)
            return;

        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0f;
            DecideMode();
        }
    }

    public void StartBossFight()
    {
        bossFightStarted = true;
        DecideMode();
    }

    void DecideMode()
    {
        float total = meleeAttacks + rangeAttacks + 0.001f;

        float normalizedMelee = meleeAttacks / total;
        float normalizedRange = rangeAttacks / total;

        Tensor input = new Tensor(1, 2);
        input[0] = normalizedMelee;
        input[1] = normalizedRange;

        worker.Execute(input);
        Tensor output = worker.PeekOutput();

        float decision = output[0];

        if (decision > 0.5f)
            boss.SetMode(BossMode.Aggressive);
        else
            boss.SetMode(BossMode.Passive);

        input.Dispose();
        output.Dispose();
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }

    // Attack tracking
    public void RegisterMeleeAttack()
    {
        meleeAttacks++;
    }

    public void RegisterRangeAttack()
    {
        rangeAttacks++;
    }
}

