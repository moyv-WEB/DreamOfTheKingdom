using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Ãæ°å")]
    public GameObject gameplayPanel;

    public GameObject gameWinPanel;
    public GameObject gamePlayerPanel;

    public GameObject gameOverPanel;

    public GameObject pickCardPanel;

    public GameObject restRoomPanel;

    public GameObject ShopPanel;
    public Gold gold;

    public void OnLoadRoomEvent(object data) 
    {
        Room currentRoom= (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                ShopPanel.SetActive(true);
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);
                break;
            case RoomType.Treasure:
                gold.Obtain(300);
                break;
        }
    }

    /// <summary>
    /// loadmap Event/load menu
    /// </summary>
    public void HideAllPanels() 
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
        ShopPanel.SetActive(false);
    }

    public void OnGameWinEvent() 
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
        gamePlayerPanel.SetActive(false);
    }
    public void OnGameOverEvent()
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gamePlayerPanel.SetActive(false);
    }
    public void OnPickCardEvent() 
    {
        pickCardPanel.SetActive(true);
       

    }
    public void OnFinishPickCardEvent() 
    {
        pickCardPanel.SetActive(false);
    }

}
