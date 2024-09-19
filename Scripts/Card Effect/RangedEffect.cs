using UnityEngine;

[CreateAssetMenu(fileName = "RangedEffect", menuName = "Effects/RangedEffect")]
public class RangedEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {

        if (target == null) { return; }
        int damage = value; // 远程攻击可能不考虑力量加成
        switch (targetType)
        {

            case EffectTargetType.Target:
                
                target.TakeDamage(damage); // 应用伤害
                break;
            case EffectTargetType.ALL:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}