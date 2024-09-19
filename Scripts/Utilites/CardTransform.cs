using UnityEngine;

public class CardTransform 
{
    public Vector3 pos;
    public Quaternion rotation;
    public CardTransform(Vector3 position, Quaternion quaternion)
    {
        pos = position;
        rotation = quaternion;
    }
}
