using NUnit.Framework.Interfaces;
using UnityEngine;

public class Boss : Entity
{
    public Boss_MoveState moveState { get; private set; }
    public Boss_DeadState deadState { get; private set; }
    public Boss_PlayerDetectedState playerDetectedState { get; private set; }
    public Boss_MeleeAttackState meleeAttackState { get; private set; }
    public Boss_IdleState idleState { get; private set; }
    public Boss_RangeAttackState rangeAttackState { get; private set; }

    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_RangeAttackState rangeAttackStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;

    [SerializeField] private float meleeCooldown = 5f;
    [SerializeField] private float rangeCooldown = 10f;
    [SerializeField] private float flipCooldown = 1.5f;
    private float lastMeleeAttackTime = -Mathf.Infinity;
    private float lastRangeAttackTime = -Mathf.Infinity;
    private float lastFlipTime = -Mathf.Infinity;



    public override void Start()
    {
        base.Start();
        Flip();

        moveState = new Boss_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new Boss_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        deadState = new Boss_DeadState(this, stateMachine, "Dead", deadStateData, this);
        idleState = new Boss_IdleState(this, stateMachine, "Idle", idleStateData, this);
        meleeAttackState = new Boss_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        rangeAttackState = new Boss_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);

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

    public bool CanMeleeAttack()
    {
        return Time.time >= lastMeleeAttackTime + meleeCooldown;
    }

    public bool CanRangeAttack()
    {
        return Time.time >= lastRangeAttackTime + rangeCooldown;
    }

    public bool CanFlip()
    {
        return Time.time >= lastFlipTime + flipCooldown;
    }

    public void SetMeleeAttackOnCooldown()
    {
        lastMeleeAttackTime = Time.time;
    }

    public void SetRangeAttackOnCooldown()
    {
        lastRangeAttackTime = Time.time;
    }

    public void SetFlipOnCooldown()
    {
        lastFlipTime = Time.time;
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
