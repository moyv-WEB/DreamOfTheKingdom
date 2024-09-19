
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="CardLibrarySO",menuName ="Card/CardLibrarySO")]
public class CardLibrarySO : ScriptableObject
{
   public List<CardLibraryEntry> cardLibraryList;
}
[System.Serializable]
public struct CardLibraryEntry 
{
    public CardDataSO cardData;//数据
    public int amount;//数量
}
