using UnityEngine;
using UnityEngine.AddressableAssets;

public class initLoad : MonoBehaviour
{
    public AssetReference persistent;

    private void Awake()
    {
        Addressables.LoadSceneAsync(persistent);
    }
}
