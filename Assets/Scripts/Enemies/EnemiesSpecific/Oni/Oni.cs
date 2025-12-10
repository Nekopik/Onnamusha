using UnityEngine;

public class Oni : Entity
{
    public Oni_IdleState idleState {  get; private set; }
    public Oni_MoveState moveState { get; private set; }
    public Oni_PlayerDetectedState playerDetectedState { get; private set; }
    public Oni_ChargeState chargeState { get; private set; }
    public Oni_LookForPlayerState lookForPlayerState { get; private set; }
    public Oni_MeleeAttackState meleeAttackState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        moveState = new Oni_MoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new Oni_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new Oni_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        chargeState = new Oni_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        lookForPlayerState = new Oni_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Oni_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (meleeAttackPosition == null || meleeAttackStateData == null)
            return;

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
