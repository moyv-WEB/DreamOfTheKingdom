using UnityEngine;
using UnityEngine.EventSystems;
public class CardDragHander :MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;
    private Card currentCard;
   
    public bool canMove;
    public bool canExecute;
    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable) { return; }
        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab,transform.position,Quaternion.identity);
                break;
            case CardType.Defense:

            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable) { return; }
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);//屏幕坐标
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//转换为世界坐标
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else 
        {
            if (eventData.pointerEnter == null) return;
            if (eventData.pointerEnter.CompareTag("Enemy")) 
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable) { return; }
        if (currentArrow !=null) { Destroy(currentArrow); }
        if (canExecute)
        {
            currentCard.ExecuteCardEffects(currentCard.player,targetCharacter);
        }
        else
        {
            currentCard.ResCardtransform();
        }
      
       
    }


}
