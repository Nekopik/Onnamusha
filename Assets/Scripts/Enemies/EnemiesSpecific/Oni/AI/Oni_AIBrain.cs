using Unity.Barracuda;
using UnityEngine;
using static Oni;
using static Oni_FightTracker;

public class Oni_AIBrain : MonoBehaviour
{
    [SerializeField] private NNModel oniModeModel;

    private Model runtimeModel;
    private IWorker worker;

    [SerializeField] private Oni oni;
    [SerializeField] private Oni_FightTracker oniTracker;

    [SerializeField] private float decisionInterval = 5f;
    private float decisionTimer;

    void Start()
    {
        runtimeModel = ModelLoader.Load(oniModeModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
    }

    void Update()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0f;
            DecideOniMode();
        }
    }

    public void DecideOniMode()
    {

    }

}
