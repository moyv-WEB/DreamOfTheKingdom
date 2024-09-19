using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI.MessageBox;


public class ShopCard : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;
    private CardDataSO currentCardDate;
    //public CardManger cardManger;
    public CardLibrarySO ShopCardLibrary;
    public CardLibrarySO PlayerHoldLibrary;
    private VisualElement rootElement;
    private Label balanceLabel;
    private Label LabelMana;
    private Label LabelGold;

    public VisualTreeAsset cardTemplate;
    private List<VisualElement> Cardgoods = new List<VisualElement>();
    private List<Label> buttonTexts = new List<Label>();


    private List<Button> buyGoodsButtons = new List<Button>();
    public Gold gold;
    public Player player;
    private int energyCost = 50; // ��ʼ��������۸�

    [Header("�¼��㲥")]
    public ObjectEventSO finishPickCardEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        balanceLabel = rootElement.Q<Label>("Balance");
        LabelMana = rootElement.Q<Label>("LabelMana");
        LabelGold = rootElement.Q<Label>("LabelGold");
        for (int a = 0; a < ShopCardLibrary.cardLibraryList.Count; a++)
        {
            int item = UnityEngine.Random.Range(0, ShopCardLibrary.cardLibraryList.Count);
            // ����Ҫ�����Ŀ���
            CardLibraryEntry cardToSwap = ShopCardLibrary.cardLibraryList[item];
            // ���н���
            ShopCardLibrary.cardLibraryList[item] = ShopCardLibrary.cardLibraryList[a];
            ShopCardLibrary.cardLibraryList[a] = cardToSwap;
            // ���������Ŀ�������
            Debug.Log(ShopCardLibrary.cardLibraryList[a].cardData.name);
        }
        for (int a = 0; a < ShopCardLibrary.cardLibraryList.Count; a++)
        {

            Debug.Log(ShopCardLibrary.cardLibraryList[a].cardData.name);
        }

        InitializeList();
        InitializeButtons();
        InitializeCards();
    }

    private void InitializeList()
    {
        buyGoodsButtons.Add(rootElement.Q<Button>("BuygoodsButton1"));
        buyGoodsButtons.Add(rootElement.Q<Button>("BuygoodsButton2"));
        buyGoodsButtons.Add(rootElement.Q<Button>("BuygoodsButton3"));
        buttonTexts.Add(rootElement.Q<Label>("buttonText1"));
        buttonTexts.Add(rootElement.Q<Label>("buttonText2"));
        buttonTexts.Add(rootElement.Q<Label>("buttonText3"));
        Cardgoods.Add(rootElement.Q<VisualElement>($"Cardgoods1"));
        Cardgoods.Add(rootElement.Q<VisualElement>($"Cardgoods2"));
        Cardgoods.Add(rootElement.Q<VisualElement>($"Cardgoods3"));
    }

    private void InitializeButtons()
    {
       
        var backtrackButton = rootElement.Q<Button>("BacktrackButton");
        var buyGoodsButton = rootElement.Q<Button>("BuygoodsButton"); // ȷ�������ť����������ȷ��

        backtrackButton.clicked += OnBacktrackButtonClicked;
        buyGoodsButton.clicked += OnBuyEnergyButtonClicked;

        for (int i = 0; i < buyGoodsButtons.Count; i++)
        {
            Debug.Log(buyGoodsButtons.Count);
            Debug.Log(i);
            var data = GetCardData(i);
            if (data != null)
            {
               
                Button buyGoodsButtone = buyGoodsButtons[i];
                Label text = buttonTexts[i]; 
                buyGoodsButtons[i].clicked += () => OnCardClicked(buyGoodsButtone, data, text);
            }
            else
            {
                Debug.LogError("Card data is null for index: " + i);
            }
        }
    }

    private void InitializeCards()
    {
        
      
        for (int i = 0; i <Cardgoods.Count; i++)
        {

            var card = cardTemplate.Instantiate();
            var data = GetCardData(i);
            if (data != null)
            {
                InitCard(card, data, Cardgoods[i]);
               
            }
            else
            {
                Debug.LogError("Card data is null for index: " + i);
            }
        }
    }

    private void OnCardClicked(Button buyGoodsButton, CardDataSO data, Label text)
    {
       
       
       
        currentCardDate = data;
       
        if (gold.CurrentGold >= 100)
        {
            
            gold.BuyGod(100);

           
            UnlockCard(currentCardDate);
        }
        else
        {
            if (text != null)
            {
                // �ı��ı���ɫ
                text.style.color = new Color(1, 0, 0);
                buyGoodsButton.SetEnabled(false);
            }
            else
            {
                Debug.LogError("Label component not found on the button.");
            }
            

          
            
        }
    }

    private void UnlockCard(CardDataSO currentCardDate)
    {
       
            var newCard = new CardLibraryEntry
            {
                cardData = currentCardDate,
                amount = 1
            };

            // ���Ҿ�����ͬcardName�Ŀ��Ƶ�����
            int index = PlayerHoldLibrary.cardLibraryList.FindIndex(t => t.cardData.cardName == currentCardDate.cardName);

            if (index != -1)
            {
                // ����ҵ��ˣ�����amount
                var cardEntry = PlayerHoldLibrary.cardLibraryList[index]; // ��ȡ��ǰ��CardLibraryEntry
                cardEntry.amount++; // �޸�amount
                PlayerHoldLibrary.cardLibraryList[index] = cardEntry; // ���޸ĺ��CardLibraryEntryд���б�
                Debug.Log("���¿�������: " + cardEntry.cardData.cardName + ", ������: " + cardEntry.amount);
            }
            else
            {
                // ���û���ҵ�������¿���
                PlayerHoldLibrary.cardLibraryList.Add(newCard);
                Debug.Log("����¿���: " + newCard.cardData.cardName);
            }
       
        

    }

    private void OnBacktrackButtonClicked()
    {
        Cardgoods.Clear();
        buttonTexts.Clear();
        buyGoodsButtons.Clear();
        loadMapEvent.RaisEvent(null, this);
    }

    private void OnBuyEnergyButtonClicked()
    {
        var buyEnergyButton = rootElement.Q<Button>("BuygoodsButton");
     
        if (gold.CurrentGold >= energyCost)
        {
            gold.BuyGod(energyCost);
            player.Obtain(1);
            energyCost += 100;
            
        }
        else
        {
            LabelGold.style.color = new Color(1, 0, 0);
            
            Debug.Log("Not enough gold");
        }
    }

   
        
    public CardDataSO GetCardData(int index)
    {
        Debug.Log($"{index}<{ShopCardLibrary.cardLibraryList.Count}");
        if (index >= 0 && index <=ShopCardLibrary.cardLibraryList.Count)
        {
           
            return ShopCardLibrary.cardLibraryList[index].cardData;
        }
        else
        {
          
            Debug.LogError("Card index out of range");
            return null;
        }
    }

    public void InitCard(VisualElement card, CardDataSO cardData, VisualElement Cardgoods )
    {
        card.dataSource = cardData;
        var cardDescription = card.Q<Label>(name: "CardDescription");
        var cardType = card.Q<Label>(name: "CardType");
        var cardCost = card.Q<Label>(name: "EnergyCost");
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        var cardSpriteElement = card.Q<VisualElement>(name: "CardSprite");
        var cardName = card.Q<Label>(name: "CardName");

        cardDescription.text = cardData.description;
        
        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardCost.text = cardData.cost.ToString();
        cardName.text = cardData.cardName;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "����",
            CardType.Defense => "����",
            CardType.Abilities => "����",
            _ => throw new System.NotImplementedException()
        };
        Cardgoods.Add(card);
    }

    private void Update()
    {
       
        balanceLabel.text =  $"{gold.CurrentGold}";
        LabelGold.text = $" {energyCost}";
        LabelMana.text = $" {player.maxMana}";
    }
}