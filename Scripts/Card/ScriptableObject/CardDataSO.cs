using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "CardDateSO",menuName = "Card/CardDateSO")]
public class CardDataSO :ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;
    [TextArea]    
    public string description;

    //执行的效果
    public List<Effect> effects;

    public static implicit operator Button(CardDataSO v)
    {
        throw new NotImplementedException();
    }
}
