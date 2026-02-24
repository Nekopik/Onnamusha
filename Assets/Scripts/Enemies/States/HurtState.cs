using UnityEngine;

public class HurtState : State
{
    protected D_HurtState stateData;

    protected bool isHurtTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;

    public HurtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_HurtState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        isHurtTimeOver = false;
        isMovementStopped = false;

        {
            base.LogicUpdate();

            if (Time.time >= startTime + stateData.hurtTime)
            {
                isHurtTimeOver = true;
            }

            if (isGrounded &&
                Time.time >= startTime + stateData.hurtKnockbackTime &&
                !isMovementStopped)
            {
                isMovementStopped = true;
                entity.SetVelocity(0f);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.hurtTime)
        {
            isHurtTimeOver = true;
        }

        if (isGrounded &&
            Time.time >= startTime + stateData.hurtKnockbackTime &&
            !isMovementStopped)
        {
            isMovementStopped = true;
            entity.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
