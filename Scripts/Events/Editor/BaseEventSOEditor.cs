
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;
    private void OnEnable()
    {

        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
      
    }
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUILayout.LabelField("订阅数量:" + GetListeners().Count);
        foreach (var Listener in GetListeners())
        {

           
            EditorGUILayout.LabelField(Listener.ToString());//显示监听器的名称
        }
    }

    private List<MonoBehaviour> GetListeners()
    {

        List<MonoBehaviour> Listeners = new();
        if (baseEventSO == null || baseEventSO.OnEventRaised == null)
        {
            return Listeners;
        }
        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();
        if (subscribers == null) { Debug.Log("subscribers"); }
        foreach (var subscriber in subscribers)
        {
           
            var obj = subscriber.Target as MonoBehaviour;
            if (!Listeners.Contains(obj))
            {
                
                Listeners.Add(obj);
            }

        }
        return Listeners;
    }

}