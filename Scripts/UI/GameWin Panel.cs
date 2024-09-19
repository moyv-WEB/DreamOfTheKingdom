using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button backToMapButton;
    [Header("�¼��㲥")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void OnEnable()
    {
        Debug.Log("�������");
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
      
        backToMapButton = rootElement.Q<Button>("BackToMapButton");
       
        backToMapButton.clicked += OnBackToMapButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
    }
    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaisEvent(null, this);
        Debug.Log("���ѡ���¼�");
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
