
using System;
[Flags]
public enum RoomType
{
    MinorEnemy  =1,//普通怪
    EliteEnemy  =2,//精英怪
    Shop        =4,      //商店
    Treasure    =8,  //宝箱
    RestRoom    =16,  //休息房间
    Boss        =32
}
public enum RoomState
{ 
    Locked,      //上锁
    Visited,    //游览
    Attainable  //可以达到的

}
public enum CardType 
{
    Attack,         //攻击
    Defense,        //防守
    Abilities      //能力
}
public enum EffectTargetType 
{
    self,//玩家
    Target, //敌人
    ALL
}