using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header(header: "组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText,descriptionText,typeText,cardnameText;
    public CardDataSO cardData;
    [Header(header: "原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public int originalLayerOrder;
    public bool isAnimating;
    public bool isAvailiable;
    public Player player;
  
    [Header("广播事件")]
    public ObjectEventSO discardCardEvent;

    public IntEventSO costEvent;
    public void Start()
    {
        Init(cardData);
       
    }
    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite=data.cardImage;
        costText.text=data.cost.ToString();
        descriptionText.text=data.description;
        cardnameText.text=data.cardName; 
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }
    public void UpdatePositionRotation(Vector3 position,Quaternion ratation)
    {
        originalPosition= position;
        originalRotation= ratation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating) return;
       transform.position = originalPosition+Vector3.up;
        transform.rotation = Quaternion.identity ;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;
        ResCardtransform();
    }
    public void ResCardtransform() 
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
        isAnimating = false;
    }
    public void ExecuteCardEffects(CharacterBase form,CharacterBase target)
    {
        //减少对应能量，通知回收卡牌
        costEvent.RaisEvent(cardData.cost, this);
        discardCardEvent.RaisEvent(this, this);
        foreach (var effect in cardData.effects)
        {

            effect.Execute(form, target);

        }
       
    }
    public void UpdateCardState()
    {
       
      
        isAvailiable = cardData.cost <= player.CurrentMana;
        costText.color=isAvailiable?Color.green:Color.red;
       

    }
    


}