using NUnit.Framework.Interfaces;
using UnityEngine;

public class Boss : Entity
{
    public Boss_DeadState deadState { get; private set; }
    public Boss_PlayerDetectedState playerDetectedState { get; private set; }
    public Boss_MeleeAttackState meleeAttackState { get; private set; }
    public Boss_IdleState idleState { get; private set; }

    [SerializeField] private D_PlayerDetectedState playerDetectedData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_IdleState idleStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;



    public override void Start()
    {
        base.Start();
        Flip();

        playerDetectedState = new Boss_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        deadState = new Boss_DeadState(this, stateMachine, "Dead", deadStateData, this);
        idleState = new Boss_IdleState(this, stateMachine, "Idle", idleStateData, this);
        meleeAttackState = new Boss_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);

        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        /*
        else if (isStuned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        */
    }


    /*
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (meleeAttackPosition == null || meleeAttackStateData == null)
            return;

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    */
}
