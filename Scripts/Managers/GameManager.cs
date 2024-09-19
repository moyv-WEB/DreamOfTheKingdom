using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Gold gold;
    [Header(header: "地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList=new();

    [Header("事件广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;
    /// <summary>
    /// 更新房间的事件监听，加载地图
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutDate(object value)
    {
        var roomVector = (Vector2Int)value;
        if (mapLayout.mapRoomDataList.Count==0) { return; }
    var currentRoom=mapLayout.mapRoomDataList.Find(r => r.colum==roomVector.x&&r.line==roomVector.y);
        currentRoom.roomState=RoomState.Visited;
        //更新相邻房间的数据
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r=>r.colum==currentRoom.colum);
        foreach (var room in sameColumnRooms) 
        {
            if(room.line != roomVector.y)
            { room.roomState = RoomState.Locked; }
              
        }
        foreach (var link in  currentRoom.linkTo) 
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r=>r.colum==link.x&&r.line==link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
        aliveEnemyList.Clear();
    }

    public void OnRoomLoadedEvent(object obj) 
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies) 
        {
            aliveEnemyList.Add(enemy);
        }
   }
    public void OnCharacterDeadEvent(object character)
    {
        if (character == null) { Debug.Log("发出错误");return; }
        if (character is Player)
        {
            //发出死亡通知 
            Debug.Log("发出死亡通知 ");
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        if (character is MinorEnemy || character is EliteEnemy) 
        {

            //获取金币
            if (character is MinorEnemy)
            {
                gold.Obtain(100);

            }
            else { gold.Obtain(200); }
            //发出胜利通知
            aliveEnemyList.Remove(character as Enemy);
            Debug.Log($"aliveEnemyList.Count:{aliveEnemyList.Count} ");
            if (aliveEnemyList.Count == 0 || aliveEnemyList != null) 
            {
                //发出胜利通知
                Debug.Log("发出胜利通知 ");
                StartCoroutine(EventDelayAction(gameWinEvent));

            }
        }
        if (character is BossEnemy)
        {
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaisEvent(null,this);
    
    }
    public void OnNewGameEvent() 
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();

    }
}
