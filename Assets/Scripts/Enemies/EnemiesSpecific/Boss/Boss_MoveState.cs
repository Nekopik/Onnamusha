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

        if (boss.player == null) return;

        float actualBossX = boss.transform.Find("Alive").position.x;
        int directionToPlayer = boss.player.position.x > actualBossX ? 1 : -1;

        if (directionToPlayer != entity.facingDirection && boss.CanFlip())
        {
            entity.Flip(); // Ensure Flip() rotates the ROOT "Boss" object
            boss.SetFlipOnCooldown();
        }

        float meleePref = boss.aiMeleePreference;

        
        if (boss.CanMakeAttackDecision())
        {
            if (isPlayerInMinAggroRange && meleePref > 0.6f && boss.CanMeleeAttack() && boss.isFightActive)
            {
                boss.MarkAttackDecision();
                stateMachine.ChangeState(boss.meleeAttackState);
                return;
            }

            // Passive mode
            if (meleePref < 0.4f && boss.CanRangeAttack() && boss.isFightActive)
            {
                boss.MarkAttackDecision();
                stateMachine.ChangeState(boss.rangeAttackState);
                return;
            }

            // In between mode
            if (meleePref >= 0.4f && meleePref <= 0.6f && boss.isFightActive)
            {
                float roll = Random.value;

                if (roll < meleePref && isPlayerInMinAggroRange && boss.CanMeleeAttack())
                {
                    boss.MarkAttackDecision();
                    stateMachine.ChangeState(boss.meleeAttackState);
                    return;
                }
                else if (boss.CanRangeAttack())
                {
                    boss.MarkAttackDecision();
                    stateMachine.ChangeState(boss.rangeAttackState);
                    return;
                }
            }
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
