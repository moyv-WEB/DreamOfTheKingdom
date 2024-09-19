using UnityEngine;

[CreateAssetMenu(fileName = "MeleeEffect", menuName = "Effects/MeleeEffect")]
public class MeleeEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
       
        if (target == null) { return; }
        switch (targetType)
        {

            case EffectTargetType.Target:
                var demage = (int)Mathf.Round(value * from.baseStrength);
                target.TakeDamage(demage);

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