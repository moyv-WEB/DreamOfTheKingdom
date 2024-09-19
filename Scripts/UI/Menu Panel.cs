
using UnityEngine;

using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement roomElement;
    private Button newGameButton, quitGameButton;
    public ObjectEventSO newGameEvent;
    private void OnEnable()
    {
        roomElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton=roomElement.Q<Button>("NewGameButton");
        quitGameButton = roomElement.Q<Button>("QuitGameButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        quitGameButton.clicked += OnQuitGameButtonClicked;
    }

   

    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaisEvent(null,this);
    }
    private void OnQuitGameButtonClicked() => Application.Quit();
    
}
