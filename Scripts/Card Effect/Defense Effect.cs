using UnityEngine;
[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.self) 
        {
            from.UpdateDefense(value);

        }
        if (targetType == EffectTargetType.Target) 
        {
            target.UpdateDefense(value);
        }
    }
}
