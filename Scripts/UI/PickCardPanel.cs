using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManger cardManger;
    private VisualElement rootElement;
    public VisualTreeAsset cardTempLate;
    private VisualElement cardContainer;
    private CardDataSO currentCardDate ;

    private Button  confirmButton;
    private List<Button> cardButtons = new();
    [Header("事件广播")]
    public ObjectEventSO finishPickCardEvent;
    private void OnEnable() 
    {
        rootElement= GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmButtonClicked;
        for (int i = 0; i < 3; i++) 
        {
            var card = cardTempLate.Instantiate();
            var data = cardManger.GetNewCardData();
            //初始化

            InitCard(card,data);
            var cardButton = card.Q<Button>("CardButton");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);  
           
            cardButton.clicked += () => OnCardClicked(cardButton,data);
            
        }


    }

    private void OnConfirmButtonClicked()
    {

        cardManger.UnlockCard(currentCardDate);
        finishPickCardEvent.RaisEvent(null,this);
    }

    private void OnCardClicked(Button cardButton,CardDataSO data)
    {
        currentCardDate = data;
        
        //Debug.Log("Card clicked"+currentCardDate.cardName);
        for (int i=0;i<cardButtons.Count; i++)
        {
            
           
            if (cardButtons[i]==cardButton) 
            {

                cardButtons[i].SetEnabled(false);
            }
            else {  cardButtons[i].SetEnabled(true); }
        }
    }

    public void InitCard(VisualElement card,CardDataSO cardData)
    {
        card.dataSource = cardData;
        var cardDescription = card.Q<Label>(name: "CardDescription");
        var cardType = card.Q<Label>(name: "CardType");
        var cardCost = card.Q<Label>(name: "EnergyCost");
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        var cardSpriteElement = card.Q<VisualElement>(name: "CardSprite");
        var cardName = card.Q<Label>(name: "CardName");

        cardDescription.text = cardData.description;
        cardContainer = rootElement.Q<VisualElement>(name: "Container");
        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardCost.text = cardData.cost.ToString();
        cardName.text = cardData.cardName;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException()
        };
      
        cardContainer.Add(card);
    }
    

}
