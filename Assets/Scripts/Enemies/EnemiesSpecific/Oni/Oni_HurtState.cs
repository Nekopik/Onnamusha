using UnityEngine;

public class Oni_HurtState : HurtState
{
    private Oni oni;

    public Oni_HurtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_HurtState stateData, Oni oni) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.oni = oni;
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

        if (isHurtTimeOver)
        {
            if (entity.CheckPlayerInMinAggroRange())
            {
                stateMachine.ChangeState(oni.meleeAttackState);
            }         
            else if (entity.CheckPlayerInMaxAggroRange())
            {
                stateMachine.ChangeState(oni.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(oni.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
