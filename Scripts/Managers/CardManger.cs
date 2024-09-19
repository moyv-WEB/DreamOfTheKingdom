using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;
using System.Collections;


public class CardManger : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;//游戏中所有可能出现的卡牌
    [Header(header: "卡牌库")]
    public CardLibrarySO newGameCardLibrary;//新游戏时初始化玩家卡牌库
    public CardLibrarySO currentLibrary;//当前玩家卡牌库
    private int previousIndex;
    private void Start()
    {
        InitializeCardDataList();
        //初始化游戏库
        foreach (var item in newGameCardLibrary.cardLibraryList) 
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }
    #region 获取项目卡牌
    /// <summary>

    /// 初始化获得所有项目卡牌资源
    /// </summary>
    private void InitializeCardDataList() 
    {
        Addressables.LoadAssetsAsync<CardDataSO>(key:"CardData", callback:null).Completed += OnCardDataLoaded;

    }
    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)//判断加载资源是否成功
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else 
        {
            Debug.LogError(message:"NO CarData Found!");
        }
    }
    #endregion
    /// <summary>
    /// 抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale= Vector3.zero;
        return cardObj;
    }
    public void DiscardCard(GameObject cardObj) 
    {
        poolTool.ReturnObjectToPool(cardObj);
    }
    public CardDataSO GetNewCardData() 
    {
        var randomIndex = 0;
        do
        {
            randomIndex =UnityEngine.Random.Range(0, cardDataList.Count);
           
        } while (previousIndex == randomIndex);
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
        
    }
    /// <summary>
    /// 解锁添加新卡
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData) 
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1
        };

        // 查找具有相同cardName的卡牌的索引
        int index = currentLibrary.cardLibraryList.FindIndex(t => t.cardData.cardName == newCardData.cardName);

        if (index != -1)
        {
            // 如果找到了，增加amount
            var cardEntry = currentLibrary.cardLibraryList[index]; // 获取当前的CardLibraryEntry
            cardEntry.amount++; // 修改amount
            currentLibrary.cardLibraryList[index] = cardEntry; // 将修改后的CardLibraryEntry写回列表
            Debug.Log("更新卡牌数量: " + cardEntry.cardData.cardName + ", 新数量: " + cardEntry.amount);
        }
        else
        {
            // 如果没有找到，添加新卡牌
            currentLibrary.cardLibraryList.Add(newCard);
            Debug.Log("添加新卡牌: " + newCard.cardData.cardName);
        }

        //var newCard = new CardLibraryEntry
        //{
        //    cardData = newCardData,
        //    amount = 1,
        //};
        //if (currentLibrary.cardLibraryList.Contains(newCard))
        //{

        //    var target = currentLibrary.cardLibraryList.Find(t => t.cardData == newCardData);
        //    target.amount++;

        //}
        //else
        //{
        //    currentLibrary.cardLibraryList.Add(newCard);
        //}
    }


}
