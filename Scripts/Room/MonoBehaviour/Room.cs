using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    [Header(header: "�㲥")]
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
            //�������¼�
           // Debug.Log("�������" + roomData.roomType);
            if (roomState == RoomState.Attainable)
            { LoadRoomEvent.RaisEvent(this, this); }//���ݹ㲥�¼�
        }
        else
        {
            Debug.LogError("roomDataΪ�գ������Ƿ�ֵ");
        }
    }

   ///<summary>
   /// �ⲿ��������ʱ�������÷���
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

