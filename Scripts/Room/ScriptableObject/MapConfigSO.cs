using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu( fileName ="MapConfigSO",menuName ="Map/MapConFigSO")]

public class MapConfigSO :ScriptableObject
{
    public List<RoomBlueprint> roomBlueprints;
}
[System.Serializable]
public class RoomBlueprint
{
    public int min, max;
    public RoomType roomType;

}
