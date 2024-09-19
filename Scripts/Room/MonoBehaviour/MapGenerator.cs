using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{

    [Header(header:"��ͼ���ñ�")]
    public MapConfigSO mapConfig;
    [Header(header: "��ͼ����")]
   public MapLayoutSO maplayout;
    [Header(header: "Ԥ����")]
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatPoint;
    public float border;
    private List<Room> rooms=new();
    private List<LineRenderer> lines = new();
    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType,RoomDataSO> roomDataDict = new();
    
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;//��Ļ�߶�
        screenWidth = screenHeight * Camera.main.aspect;//��Ļ���

        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 0.25f);
        foreach (var roomData in roomDataList) 
        {
            roomDataDict.Add(roomData.roomType,roomData);
        }
    }
    //private void Start()
    //{
    //    CreateMap();
    //}
    private void OnEnable()
    {
        
        if (maplayout.mapRoomDataList.Count > 0)
        {
            
            LoadMap();
        }
        else { CreateMap(); }
    }
    public void CreateMap()
    {
        //����ǰһ�з����б�
        List<Room> previousColumnRooms = new ();
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatPoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, z: 0);//��ʼ���ɵ�
            var newPosition = generatPoint;
            //������ǰ�з����б�
            List<Room> currentColumnRooms = new();
            var roomGapY = screenHeight / (amount + 1);
            //ѭ����ǰ�����з�����������ɷ���
            for (int i = 0; i < amount; i++)
            {
                //�ж����һ�У�ΪBoss��
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }
                else if (column != 0)
                {
                    newPosition.x = generatPoint.x + UnityEngine.Random.Range(-border / 2, border / 2);
                }
               
               
                newPosition.y = startHeight - roomGapY * i;
                //���ɷ���
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);

                //����ֻ�е�һ�з�����Խ��������������
                if (column == 0) { room.roomState = RoomState.Attainable; }
                else { room.roomState = RoomState.Locked; }

                room.SetupRoom(column,i,GetRoomData(newType) );  
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            //�жϵ�ǰ���Ƿ�Ϊ��һ�У�������������ӵ���һ��
            if (previousColumnRooms.Count>0)
            {
                //���������б�ķ�������
                CreateConnetions(previousColumnRooms, currentColumnRooms);
                
            }
            previousColumnRooms = currentColumnRooms;
        }
        SaveMap();
    }

    private void CreateConnetions(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new();
        foreach (var room in column1)
        {
            var targetRoom=ConnectToRandomRoom(room, column2, check: false);
            connectedColumn2Rooms.Add(targetRoom);
        }
        //����ȷ��column2�����еķ��䶼�����ӵķ���
        foreach (var room in column2)
        {
            // �жϷ����Ƿ񲻰��������ӵ�����
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room,column1, check: true);
            }
        }

    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2,bool check)
    {
        Room targetRoom;
       
        targetRoom = column2[UnityEngine.Random.Range(minInclusive: 0, column2.Count)];
        if (check)
        {
            targetRoom.linkTo.Add(new(room.column,room.line));
        }
        else
        {
            room.linkTo.Add(new(targetRoom.column, targetRoom.line));
        }
        //��������֮������
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0,room.transform.position);//ʼ
        line.SetPosition(1,targetRoom.transform.position);//��
        lines.Add(line);
        return targetRoom;
    }
    //�������ɵ�ͼ
    [ContextMenu(itemName: "ReGenerateRoom")]
    public void ReGenerateRoom()
    {
       
        foreach (var room in rooms)
        {
          Destroy(room.gameObject);
        }
        foreach (var itms in lines)
        {
          Destroy(itms.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();

    }
    private RoomDataSO GetRoomData(RoomType roomType)
    { 
    return roomDataDict[roomType];  
    }
    private RoomType GetRandomRoomType(RoomType flags) 
    { 
    string[] options=flags.ToString().Split(separator:',');
        string randomOption = options[UnityEngine.Random.Range(minInclusive:0,options.Length)];
           RoomType roomType=(RoomType)Enum.Parse(typeof(RoomType),randomOption);
        return roomType;
    
    }
    private void SaveMap() 
    {
        maplayout.mapRoomDataList = new();
        //��������ɵķ���
        for (int i=0;i<rooms.Count;i++) 
        {
            var room = new MapRoomDate()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo
            };
            maplayout.mapRoomDataList.Add(room);    
        }
        maplayout.linePositionList = new();
        //������е�����
        for (int i=0; i<lines.Count;i++)
        {
            var line = new LinePosition()
            {
                startPos = new SeriallizeVector3(lines[i].GetPosition(index:0)),
                endPos = new SeriallizeVector3(lines[i].GetPosition(index: 1)),
            };
            maplayout.linePositionList.Add(line);
        }
    }
    private void LoadMap() 
    {
        //��ȡ�����������ɷ���
        for (int i=0;i<maplayout.mapRoomDataList.Count; i++) 
        {
            var MRlist = maplayout.mapRoomDataList[i];
            var newPos = new Vector3(MRlist.posX, MRlist.posY,z:0);
            var newRoom = Instantiate(roomPrefab,newPos,Quaternion.identity,transform);
            newRoom.roomState = MRlist.roomState;
            newRoom.SetupRoom(MRlist.colum,MRlist.line,MRlist.roomData);
            newRoom.linkTo = MRlist.linkTo;
            rooms.Add(newRoom);

        }
        //��ȡ����
        for (int i=0;i<maplayout.linePositionList.Count;i++) 
        {
            var linePL = maplayout.linePositionList[i];
            var line = Instantiate(linePrefab,transform);
            line.SetPosition(index:0,linePL.startPos.ToVector3());
            line.SetPosition(index:1, linePL.endPos.ToVector3());
            lines.Add(line);
            
        }
    }
}

