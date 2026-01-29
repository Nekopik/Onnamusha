using UnityEngine;

public class Oni : Entity
{
    public Oni_IdleState idleState {  get; private set; }
    public Oni_MoveState moveState { get; private set; }
    public Oni_PlayerDetectedState playerDetectedState { get; private set; }
    public Oni_ChargeState chargeState { get; private set; }
    public Oni_LookForPlayerState lookForPlayerState { get; private set; }
    public Oni_MeleeAttackState meleeAttackState { get; private set; }
    public Oni_StunState stunState { get; private set; }
    public Oni_DeadState deadState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Oni_FightTracker mobFightTracker;

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
        stunState = new Oni_StunState(this, stateMachine, "Stun", stunStateData, this);
        deadState = new Oni_DeadState(this, stateMachine, "Dead", deadStateData, this);

        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (!mobFightTracker.mobFightActive)
            mobFightTracker.StartFight();

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
            mobFightTracker.EndFight();
        }
        else if (isStuned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        } 
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (meleeAttackPosition == null || meleeAttackStateData == null)
            return;

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
