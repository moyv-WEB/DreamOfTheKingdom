using UnityEngine;

public class FinishRoom : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;
    private void OnMouseDown()
    {
        //·µ»ØµØÍ¼ 
        loadMapEvent.RaisEvent(value:null,this);
    }
}
