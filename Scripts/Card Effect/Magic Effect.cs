using UnityEngine;

[CreateAssetMenu(fileName = "MagicEffect", menuName = "Effects/MagicEffect")]
public class MagicEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null) return;
        int damage = (int)Mathf.Round(value * from.baseMagic); // ħ���˺�����
        target.TakeDamage(damage); // Ӧ���˺�
    }
}