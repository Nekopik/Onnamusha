using UnityEngine;

public class Boss_RangeAttackState : RangeAttackState
{
    private Boss boss;
    public Boss_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData, Boss boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        boss.SetRangeAttackOnCooldown();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        stateMachine.ChangeState(boss.moveState);
    }
}
