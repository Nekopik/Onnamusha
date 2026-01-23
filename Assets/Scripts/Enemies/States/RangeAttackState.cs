using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState stateData;
    protected AttackDetails attackDetails;

    private Vector2 attackDirection;
    private Vector2 lockedAttackDirection;

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

        entity.SetVelocity(0f);

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGameObject.transform.position;

        //attackDirection = entity.aliveGameObject.transform.right;
        lockedAttackDirection = GetLockedPlayerDirection();

        /*
        Collider2D player = Physics2D.OverlapCircle(
            entity.transform.position,
            stateData.playerDetectedRange,
            stateData.whatIsPlayer
        );
        */
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
        GameObject projectile = GameObject.Instantiate(
            stateData.projectilePrefab,
            attackPosition.position,
            Quaternion.identity
        );

        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (projectileScript == null)
        {
            Debug.LogError("No projectile prefab");
            return;
        }

        projectileScript.Fire(
            lockedAttackDirection,
            stateData.projectileSpeed,
            attackDetails
        );
    }

    private Vector2 GetLockedPlayerDirection()
    {
        Collider2D player = Physics2D.OverlapCircle(
            attackPosition.position,
            stateData.playerDetectedRange,
            stateData.whatIsPlayer
        );

        if (player == null)
        {
            // fallback: shoot forward
            return entity.aliveGameObject.transform.right;
        }

        return (player.transform.position - attackPosition.position).normalized;
    }
}
