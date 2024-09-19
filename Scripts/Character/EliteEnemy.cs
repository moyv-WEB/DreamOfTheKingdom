using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EliteEnemy : Enemy
{
    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack1();
                Attack2();
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
        Debug.Log("skill");
    }
    public virtual void Attack1()
    {

         if (currentAction.effect is RangedEffect)
        { StartCoroutine(ProcessDelayAction("attack1"));
            Debug.Log("attack1");
        }
    }
    public virtual void Attack2()
    {
        if (currentAction.effect is MeleeEffect) 
        { StartCoroutine(ProcessDelayAction("attack2"));
            Debug.Log("attack2");
        }
       
    }
    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.5f &&
            !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack1")
        {
            Debug.Log("ProcessDelayAction(\"attack1\")");
            currentAction.effect.Execute(this, player);
        }
        else if (actionName == "attack2") 
        { currentAction.effect.Execute(this, player);
            Debug.Log("ProcessDelayAction(\"attack2\")");
        }
        else
        {
           
            currentAction.effect.Execute(this, this);
        }
    }
}
