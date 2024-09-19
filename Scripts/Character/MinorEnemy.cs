using System.Collections;
using UnityEngine;

public class MinorEnemy : Enemy
{
    public virtual void OnEnemyTurnBegin()
    {
        RestDefense();
        switch (currentAction.effect.targetType)
        {
           
            case EffectTargetType.self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.ALL:

                break;
        }
    }

    public virtual void Skill()
    {
        //animator.SetTrigger("skill");
        //currentAction.effect.Execute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }
    public virtual void Attack()
    {
        //animator.SetTrigger("attack");
        //currentAction.effect.Execute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }
    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.5f &&
            !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack")
        {
            currentAction.effect.Execute(this, player);
        }
        //if (actionName == "skill") 
        else
        {
            currentAction.effect.Execute(this, this);
        }
    }
}
