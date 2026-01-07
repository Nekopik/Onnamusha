using UnityEngine;

public class Boss_MeleeAttackState : MeleeAttackState
{
    private Boss boss;
    public Boss_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Boss boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }
}
