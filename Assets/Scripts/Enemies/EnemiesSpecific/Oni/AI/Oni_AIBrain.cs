using Unity.Barracuda;
using UnityEngine;
using static Oni;

public class Oni_AIBrain : MonoBehaviour
{
    [SerializeField] private NNModel oniModeModel;

    private Model runtimeModel;
    private Tensor inputTensor;
    private IWorker worker;

    [SerializeField] private Oni oni;
    [SerializeField] private Oni_FightTracker oniTracker;

    [SerializeField] private float decisionInterval = 5f;
    private float decisionTimer;

    void Start()
    {
        runtimeModel = ModelLoader.Load(oniModeModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        inputTensor = new Tensor(1, 4); // Reuse
    }

    private void Awake()
    {
        if (oniTracker == null)
            oniTracker = FindFirstObjectByType<Oni_FightTracker>();
    }

    void Update()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0f;
            DecideOniMode();
            Debug.Log("Oni made a mode decision");
        }
    }

    public void DecideOniMode()
    {
        //float total = oniTracker.mobMeleeAttacks + oniTracker.mobRangeAttacks + 0.001f;
        inputTensor[0, 0] = oniTracker.durationOfMobFight;
        inputTensor[0, 1] = oniTracker.mobMeleeAttacks; // / total;
        inputTensor[0, 2] = oniTracker.mobRangeAttacks; // / total;
        inputTensor[0, 3] = oniTracker.playerEndHP;
        // + add global parameters

        worker.Execute(inputTensor);
        Tensor output = worker.PeekOutput(); // Don't dispose
        float decision = output[0];

        oni.SetMobMode(decision > 0.5f ? Oni.MobMode.Aggressive : Oni.MobMode.Passive);
    }
    void OnDestroy()
    {
        inputTensor.Dispose(); // Dispose reused input
        worker.Dispose();      // Dispose worker
    }
}
