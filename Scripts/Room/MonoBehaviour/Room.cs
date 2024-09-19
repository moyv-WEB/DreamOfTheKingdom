using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    [Header(header: "广播")]
    public ObjectEventSO LoadRoomEvent;
    public List<Vector2Int> linkTo=new();
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


    }
   
    private void OnMouseDown()
    {
       
        if (roomData != null)
        {
            //处理点击事件
           // Debug.Log("点击房间" + roomData.roomType);
            if (roomState == RoomState.Attainable)
            { LoadRoomEvent.RaisEvent(this, this); }//传递广播事件
        }
        else
        {
            Debug.LogError("roomData为空，请检查是否赋值");
        }
    }

   ///<summary>
   /// 外部创建房间时调用配置房间
   ///<param name="column"></garam>
   ///<param name = "line" ></ pafam >
   ///< param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;   
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;

        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(r: 0.5f, g: 0.5f, b: 0.5f, a: 1f),
            RoomState.Visited => new Color(r: 0.5f, g: 0.8f, b: 0.5f, a: 0.5f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
       
    }
}

