using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public FadePanel FadePanel;
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;
    private Room currentRoom;
    private Vector2Int currentRoomVector;
    [Header(header:"�㲥")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;


    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;   
        //LoadMenu();
        LoadIntro();
    }
    ///<summary>
    ///�ڷ�������¼�����
    ///</summary>
    ///<param name="data"></param>

    public async void OnLoadRoomEvent(object data) 
    {
    if(data is Room)
        {
            currentRoom=data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new(currentRoom.column,currentRoom.line);
            
            currentScene = currentData.sceneToload;
        }
        //ж�ط���
        await UnloadSceneTask();
        //���ط���
        await LoadSceneTask();

        afterRoomLoadedEvent.RaisEvent(currentRoom, this);
    }
    ///<summary>
    ///�첽�������س���
    ///</summary>
    ///<returns></returns>
    private async Awaitable LoadSceneTask()
    {
       var s=   currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status==AsyncOperationStatus.Succeeded)
        {
            FadePanel.FadeOut(0.2f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }
    private async Awaitable UnloadSceneTask()
    {
        FadePanel.FadeIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
      await  Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
      
    }
    ///<summary>
    ///�������ط�����¼�����
    ///</summary>
   
    public async void LoadMap() 
    {
        await UnloadSceneTask();
        if (currentRoomVector!=Vector2.one*-1)
        { updateRoomEvent.RaisEvent(currentRoomVector,this); }
        currentScene = map;   
        await LoadSceneTask();
    }
    public async void LoadMenu()
    {
        if (currentScene != null)
        {
            await UnloadSceneTask();//ж�س���
                                                            
        }
       
     
        currentScene = menu;
        await LoadSceneTask();
    }
    public async void LoadIntro()
    {
        if (currentScene != null)
        {
            await UnloadSceneTask();//ж�س���

        }


        currentScene = intro;
        await LoadSceneTask();
    }
}
