using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState stateData;
    protected AttackDetails attackDetails;

    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGameObject.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D player = Physics2D.OverlapCircle(attackPosition.position, 50f /* TODO: change to variable later */, stateData.whatIsPlayer);
        Vector2 direction = (player.transform.position - attackPosition.position).normalized;

        GameObject projectile = GameObject.Instantiate(
            stateData.projectilePrefab,
            attackPosition.position,
            Quaternion.identity
        );

        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Fire(direction, stateData.projectileSpeed, attackDetails);
        }
    }
}
