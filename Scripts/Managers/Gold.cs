

using UnityEngine;

public class Gold : MonoBehaviour
{
    public IntVariable PlayerGold;
    public int maxgold;
    public int CurrentGold
    {
        get => PlayerGold.currentValue;
        set => PlayerGold.SetValue(value);
    }
    private void Awake()
    {
        PlayerGold.maxValue = maxgold;
        PlayerGold.currentValue = 0;//³õÊ¼½ð±Ò
        

    }
    public void BuyGod(int gold)
    {
        CurrentGold -= gold;
        if (CurrentGold <= 0)
        {
            CurrentGold = 0;
        }
        Debug.Log($"gold:{gold}");


    }
    public void Obtain(int gold)
    {
        CurrentGold += gold;
        if (CurrentGold >= maxgold)
        {
            CurrentGold = maxgold;
        }
        Debug.Log($"gold:{gold}");
    }
}
