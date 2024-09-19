using UnityEngine;
[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaisEvent(value, this);
    }
}
