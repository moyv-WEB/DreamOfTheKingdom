using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public GameObject playerObj;

    private bool isPlayerTurn=false;
    private bool isEnemyTurn=false;
    private bool battleEnd=true;

    

    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;
    [Header("事件广播")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;
   
    private void Update()
    {
        if (battleEnd) { return; }
        if (isEnemyTurn) 
        {
            timeCounter += Time.deltaTime;
            if (timeCounter>=enemyTurnDuration) 
            {
                timeCounter = 0f;
                //敌人回合结束
                isEnemyTurn = false;
                //玩家回合开始
                isPlayerTurn = true;
               
            }
        }
        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                //玩家回合开始
               
                PlayerTurnBegin();
                isPlayerTurn = false;
               
            }
        }
    }
    [ContextMenu("Game Start")]
    public void GameStart()
    {
        isPlayerTurn=true;
        isEnemyTurn=false;
        battleEnd=false;
        timeCounter = 0f;
    }
    
    public void PlayerTurnBegin()
    {
       
        playerTurnBegin.RaisEvent(null,this);

    }
    public void EnemyTurnBegin() 
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaisEvent(null,this);
    }
    public void EnemyTurnEnd() 
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaisEvent(null,this);
    }

    /// <summary>
    /// 注册时间函数 ofter room load
    /// </summary>
    /// <param name="obj"></param>
    public void OnRoomLoadedEvent(object obj) 
    {
        Room room=obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:             
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                OnPlayerStart();
                GameStart();
               
                break;
            case RoomType.Shop:               
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                playerObj.GetComponent<PlayerAnimation>().SetSleepAnimation();
                break;
            
        }
    }
    public void StopTurnBaseSystem(object obj) 
    {
        battleEnd= true;
        playerObj.SetActive(false);
    }
    private void OnPlayerStart() 
    {
        PalyerHealthBar palyerHealthBar=playerObj.GetComponent<PalyerHealthBar>();
        palyerHealthBar.InitHealthBar();
    }
    public void NewGame() 
    {
        playerObj.GetComponent<Player>().NewGame();
    }
    
}
