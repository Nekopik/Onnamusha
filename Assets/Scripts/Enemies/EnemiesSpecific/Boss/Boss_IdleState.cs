using UnityEngine;

public class Boss_IdleState : IdleState
{
    private Boss boss;
    public Boss_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState state, Boss boss) : base(entity, stateMachine, animBoolName, state)
    {
        this.boss = boss;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(boss.playerDetectedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
