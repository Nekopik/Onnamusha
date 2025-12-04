using UnityEngine;

public class Oni_IdleState : IdleState
{
    private Oni enemy;
    public Oni_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState state, Oni enemy) : base(entity, stateMachine, animBoolName, state)
    {
        this.enemy = enemy;
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

        if(isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
