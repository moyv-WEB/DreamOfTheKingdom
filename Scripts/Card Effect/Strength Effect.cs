using UnityEngine;
[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public float strength;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        from.strengthEffect = strength;
        switch (targetType)
        {
            case EffectTargetType.self:
                from.SetupStrength(value,true);
                break;
            case EffectTargetType.Target:
                target.SetupStrength(value,false);
                break;
            case EffectTargetType.ALL:
                break;
        }
    }
}
