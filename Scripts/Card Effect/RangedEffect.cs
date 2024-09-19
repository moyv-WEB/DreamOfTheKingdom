using UnityEngine;

[CreateAssetMenu(fileName = "RangedEffect", menuName = "Effects/RangedEffect")]
public class RangedEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {

        if (target == null) { return; }
        int damage = value; // Զ�̹������ܲ����������ӳ�
        switch (targetType)
        {

            case EffectTargetType.Target:
                
                target.TakeDamage(damage); // Ӧ���˺�
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