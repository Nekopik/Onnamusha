using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;
    public Entity entity;


    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    public void FinishDeath()
    {
        if (entity != null)
        {
            entity.OnDeathAnimationFinished();
        }
    }
}
