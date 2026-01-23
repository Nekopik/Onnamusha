using UnityEngine;

public class Boss_MeleeAttackState : MeleeAttackState
{
    private Boss boss;
    private Boss_AIBrain boss_AIBrain;
    public Boss_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Boss boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        Debug.Log("Boss Melee Atack State");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        boss.SetMeleeAttackOnCooldown();
        boss_AIBrain.RegisterMeleeAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
