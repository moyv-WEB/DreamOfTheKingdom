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
    public List<CardDataSO> cardDataList;//��Ϸ�����п��ܳ��ֵĿ���
    [Header(header: "���ƿ�")]
    public CardLibrarySO newGameCardLibrary;//����Ϸʱ��ʼ����ҿ��ƿ�
    public CardLibrarySO currentLibrary;//��ǰ��ҿ��ƿ�
    private int previousIndex;
    private void Start()
    {
        InitializeCardDataList();
        //��ʼ����Ϸ��
        foreach (var item in newGameCardLibrary.cardLibraryList) 
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }
    #region ��ȡ��Ŀ����
    /// <summary>

    /// ��ʼ�����������Ŀ������Դ
    /// </summary>
    private void InitializeCardDataList() 
    {
        Addressables.LoadAssetsAsync<CardDataSO>(key:"CardData", callback:null).Completed += OnCardDataLoaded;

    }
    /// <summary>
    /// �ص�����
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)//�жϼ�����Դ�Ƿ�ɹ�
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
    /// �鿨ʱ���õĺ�����ÿ���GameObject
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
    /// ��������¿�
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData) 
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1
        };

        // ���Ҿ�����ͬcardName�Ŀ��Ƶ�����
        int index = currentLibrary.cardLibraryList.FindIndex(t => t.cardData.cardName == newCardData.cardName);

        if (index != -1)
        {
            // ����ҵ��ˣ�����amount
            var cardEntry = currentLibrary.cardLibraryList[index]; // ��ȡ��ǰ��CardLibraryEntry
            cardEntry.amount++; // �޸�amount
            currentLibrary.cardLibraryList[index] = cardEntry; // ���޸ĺ��CardLibraryEntryд���б�
            Debug.Log("���¿�������: " + cardEntry.cardData.cardName + ", ������: " + cardEntry.amount);
        }
        else
        {
            // ���û���ҵ�������¿���
            currentLibrary.cardLibraryList.Add(newCard);
            Debug.Log("����¿���: " + newCard.cardData.cardName);
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
