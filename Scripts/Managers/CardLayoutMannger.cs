using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutMannger : MonoBehaviour
{
    public bool isHorzontal;
    public float maxWidth=7f;
    public float cardSpacing = 2f;

    [Header(header: "���β���")]
    public float angLeBEtweenCards = 7f;//�ǶȲ�
    public float radius = 17f;//�뾶
    public Vector3 centerPoint;
   [SerializeField] 
    private List<Vector3> cardPositions=new();
    private List<Quaternion> cardRotation = new();



    private void Awake()
    {
        centerPoint = isHorzontal?Vector3.up*-4f:Vector3.up*-20.8f;
    }
    public CardTransform GetCardTransform(int index,int totalCards) 
    {
        CalculatePosition(totalCards, isHorzontal);
        return new CardTransform(cardPositions[index], cardRotation[index]);


    }
    private void CalculatePosition(int numberOfCards,bool horizontal) 
    {
        cardPositions.Clear();
        cardRotation.Clear();
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;//��Ԫ����
            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);
                var pos = new Vector3(xPos, centerPoint.y, 0);
                var rotation = Quaternion.identity;
                cardPositions.Add(pos);
                cardRotation.Add(rotation);
            }
        }
        else
        {
            float cardAngLe = (numberOfCards - 1) * angLeBEtweenCards /2;
            for (int i = 0;i < numberOfCards;i++) 
            {
                var pos = FanCardPosition(cardAngLe - i * angLeBEtweenCards);
                var rotation=Quaternion.Euler(0, 0, cardAngLe - i * angLeBEtweenCards);
                cardPositions.Add(pos);
                cardRotation.Add(rotation);
            }
        }
    }
    private Vector3 FanCardPosition(float angle) //����ת��λ��
    {
        return new Vector3
            (
            centerPoint.x-Mathf.Sin(Mathf.Deg2Rad*angle)*radius,
            centerPoint.y+Mathf.Cos(Mathf.Deg2Rad * angle)*radius,
            z:0
            );
    }

}
