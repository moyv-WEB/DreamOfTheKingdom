using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button backToMapButton;
    [Header("事件广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void OnEnable()
    {
        Debug.Log("启动面板");
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
      
        backToMapButton = rootElement.Q<Button>("BackToMapButton");
       
        backToMapButton.clicked += OnBackToMapButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
    }
    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaisEvent(null, this);
        Debug.Log("点击选卡事件");
    }
    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaisEvent(null,this);
    }
    public void OnFinshPickCardEvent()
    {
        pickCardButton.style.display=DisplayStyle.None;
    }
}
