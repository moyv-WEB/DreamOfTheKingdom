
using System;
[Flags]
public enum RoomType
{
    MinorEnemy  =1,//��ͨ��
    EliteEnemy  =2,//��Ӣ��
    Shop        =4,      //�̵�
    Treasure    =8,  //����
    RestRoom    =16,  //��Ϣ����
    Boss        =32
}
public enum RoomState
{ 
    Locked,      //����
    Visited,    //����
    Attainable  //���Դﵽ��

}
public enum CardType 
{
    Attack,         //����
    Defense,        //����
    Abilities      //����
}
public enum EffectTargetType 
{
    self,//���
    Target, //����
    ALL
}