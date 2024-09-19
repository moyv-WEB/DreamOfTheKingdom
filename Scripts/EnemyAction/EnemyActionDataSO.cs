using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = " EnemyActionDataSO", menuName = "Enemy/EnemyAction/ EnemyActionDataSO")]
public class EnemyActionDataSO : ScriptableObject
{
    public List<EnemyAction> actions;
}
[System.Serializable]
public struct EnemyAction 
{
    public Sprite intenSprite;
    public Effect effect;
}
