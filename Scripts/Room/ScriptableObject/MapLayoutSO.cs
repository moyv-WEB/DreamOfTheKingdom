using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomDate> mapRoomDataList = new();
    public List<LinePosition> linePositionList = new();
}
[System.Serializable]
public class MapRoomDate 
{
    public float posX, posY;
    public int colum, line;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}
[System.Serializable]
public class LinePosition
{
    public SeriallizeVector3 startPos, endPos;
}
