using UnityEngine;
[CreateAssetMenu(fileName ="IntVarible",menuName = "Varible/InVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    public IntEventSO ValueChangedEvent;
    [TextArea]
    [SerializeField]private string descripion;
    public void SetValue(int value)
    {
        currentValue =value;
        ValueChangedEvent?.RaisEvent(value,this);
    }
    public void SetMaxValue(int value)
    {
        maxValue = value;
        ValueChangedEvent?.RaisEvent(value, this);
    }
}
