
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;


public class CardDeck : MonoBehaviour
{
    public CardManger CardManger;
    public CardLayoutMannger layoutManager;
    public Vector3 deckPostion;
    private List<CardDataSO> drawDeck = new();//���ƶ�
    private List<CardDataSO> discardDeck = new();//���ƶ�
    private List<Card> handCardObjectList = new();//���ƣ�ÿ�غϣ�

    
    [Header("�¼��㲥")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    private void Start()//����
    {
        InitializeDack();

    }
    public void InitializeDack()
    {
        drawDeck.Clear();
        foreach (var entry in CardManger.currentLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        //TODO:ϴ��/���³��ƶ�or���ƶѵ�����
        ShuffleDeck();
    }
    [ContextMenu("���Գ���")]
    public void TestDrawCard()
    {

        DrawCard(1);
    }
    /// <summary>
    /// �¼��������� 
    /// </summary>
    public void NewTurnDrawCards()
    {

        DrawCard(4);
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            if (drawDeck.Count == 0)
            {
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            //����UI����
            drawCountEvent.RaisEvent(drawDeck.Count, this);

            var card = CardManger.GetCardObject().GetComponent<Card>();
            //��ʼ��
            card.Init(currentCardData);
            card.transform.position = deckPostion;

            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }
    /// <summary>
    /// ���ÿ��Ʋ���
    /// </summary>
    /// <param name="delay">�ӳ�ʱ��</param>
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {

            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos,cardTransform.rotation);
            //�жϿ�������
            
            currentCard.UpdateCardState();
            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.25f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.35f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);

            };


            //���ÿ�������
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    /// <summary>
    /// ϴ��
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        ///TODO:����UI��ʾ����
        drawCountEvent.RaisEvent(drawDeck.Count, this);
        discardCountEvent.RaisEvent(discardDeck.Count, this);
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;

        }

    }
    /// <summary>
    /// �����߼����¼�����
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        CardManger.DiscardCard(card.gameObject);
        discardCountEvent.RaisEvent(discardDeck.Count, this);
        SetCardLayout(0f);

    }
    /// <summary>
    /// �¼��������� 
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            CardManger.DiscardCard(handCardObjectList[i].gameObject);
        }

        handCardObjectList.Clear();

        discardCountEvent.RaisEvent(discardDeck.Count, this);
    }
    public void ReleaseAllCards(object obj)
    {
      
        foreach (var card in handCardObjectList)
        {
            CardManger.DiscardCard(card.gameObject);
        }
        handCardObjectList.Clear();
        discardCountEvent.RaisEvent(discardDeck.Count,this);
    }
}