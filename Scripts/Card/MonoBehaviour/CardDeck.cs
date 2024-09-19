
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;


public class CardDeck : MonoBehaviour
{
    public CardManger CardManger;
    public CardLayoutMannger layoutManager;
    public Vector3 deckPostion;
    private List<CardDataSO> drawDeck = new();//抽牌堆
    private List<CardDataSO> discardDeck = new();//弃牌堆
    private List<Card> handCardObjectList = new();//手牌（每回合）

    
    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    private void Start()//测试
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

        //TODO:洗牌/更新抽牌堆or弃牌堆的数字
        ShuffleDeck();
    }
    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {

        DrawCard(1);
    }
    /// <summary>
    /// 事件监听函数 
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

            //更新UI数字
            drawCountEvent.RaisEvent(drawDeck.Count, this);

            var card = CardManger.GetCardObject().GetComponent<Card>();
            //初始化
            card.Init(currentCardData);
            card.transform.position = deckPostion;

            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }
    /// <summary>
    /// 设置卡牌布局
    /// </summary>
    /// <param name="delay">延迟时间</param>
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {

            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos,cardTransform.rotation);
            //判断卡牌能量
            
            currentCard.UpdateCardState();
            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.25f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.35f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);

            };


            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        ///TODO:更新UI显示数量
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
    /// 弃牌逻辑，事件函数
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
    /// 事件监听函数 
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