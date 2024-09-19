using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton, backToMapButton;
    private CharacterBase player;
    public Effect restEffect;
    public ObjectEventSO loadMapEvent;
    public Gold gold;
    public void OnEnable()
    {
       rootElement= GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
        restButton.clicked+= OnRestButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnBackToMapButtonClicked()
    {
        gold.Obtain(100);
        loadMapEvent.RaisEvent(null,this);
    }

    private void OnRestButtonClicked()
    {
        restEffect.Execute(player,null);
        restButton.SetEnabled(false);
    }
}
