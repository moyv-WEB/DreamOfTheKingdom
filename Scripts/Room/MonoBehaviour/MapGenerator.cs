using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{

    [Header(header:"地图配置表")]
    public MapConfigSO mapConfig;
    [Header(header: "地图布局")]
   public MapLayoutSO maplayout;
    [Header(header: "预制体")]
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
        screenHeight = Camera.main.orthographicSize * 2;//屏幕高度
        screenWidth = screenHeight * Camera.main.aspect;//屏幕宽度

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
        //创建前一列房间列表
        List<Room> previousColumnRooms = new ();
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatPoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, z: 0);//初始生成点
            var newPosition = generatPoint;
            //创建当前列房间列表
            List<Room> currentColumnRooms = new();
            var roomGapY = screenHeight / (amount + 1);
            //循环当前列所有房间的数量生成房间
            for (int i = 0; i < amount; i++)
            {
                //判断最后一列，为Boss房
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }
                else if (column != 0)
                {
                    newPosition.x = generatPoint.x + UnityEngine.Random.Range(-border / 2, border / 2);
                }
               
               
                newPosition.y = startHeight - roomGapY * i;
                //生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);

                //设置只有第一列房间可以进入其他房间的锁
                if (column == 0) { room.roomState = RoomState.Attainable; }
                else { room.roomState = RoomState.Locked; }

                room.SetupRoom(column,i,GetRoomData(newType) );  
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            //判断当前列是否为第一列，如果不是则连接到上一列
            if (previousColumnRooms.Count>0)
            {
                //创建两个列表的房间连线
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
        //检验确保column2中所有的房间都有链接的房间
        foreach (var room in column2)
        {
            // 判断房间是否不包含在连接的列中
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
        //创建房间之间连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0,room.transform.position);//始
        line.SetPosition(1,targetRoom.transform.position);//终
        lines.Add(line);
        return targetRoom;
    }
    //重新生成地图
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
        //添加以生成的房间
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
        //添加所有的连线
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
        //读取房间数据生成房间
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
        //读取连线
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

