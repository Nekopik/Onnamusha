using UnityEngine;

public class Boss_MoveState : MoveState
{
    private Boss boss;

    private float stoppingDistance = 0.2f;
    public Boss_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Boss boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        Debug.Log("Boss Move State");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAggroRange && boss.CanFlip())
        {
            entity.Flip();
            boss.SetFlipOnCooldown();
        }

        if (isPlayerInMinAggroRange && boss.CanMeleeAttack())
        {
            stateMachine.ChangeState(boss.meleeAttackState);
            return;
        }

        if (isPlayerInMaxAggroRange && boss.CanRangeAttack())
        {
            stateMachine.ChangeState(boss.rangeAttackState);
            return;
        }

        if (!boss.isFightActive)
        {
            Transform bossBasePoint = boss.basePosition;

            if (bossBasePoint != null)
            {
                float distance = Vector2.Distance(entity.transform.position, bossBasePoint.position);

                if (distance <= stoppingDistance)
                {
                    entity.SetVelocity(0f);
                    stateMachine.ChangeState(boss.idleState);
                    return;
                }

                int direction = bossBasePoint.position.x > entity.transform.position.x ? 1 : -1;
                entity.SetFacingDirection(direction);
                entity.SetVelocity(stateData.movementSpeed);
                return;
            }

            stateMachine.ChangeState(boss.idleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
