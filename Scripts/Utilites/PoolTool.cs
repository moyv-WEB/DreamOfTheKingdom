using UnityEngine;
using UnityEngine.Pool;
[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        //初始化对象池
        pool = new ObjectPool<GameObject>(
          createFunc: ()=>Instantiate(objPrefab,transform),//创建，父级设置为当前物体
          actionOnGet:(obj)=>obj.SetActive(value:true),//获取（启用）
          actionOnRelease:(obj)=>obj.SetActive(value:false),//回收
          actionOnDestroy:(obj)=>Destroy(obj),//销毁
          collectionCheck:false,
          defaultCapacity:10,//默认大小
          maxSize:20   //最大上限        
            );
        PreFillPool(7);
    }
    private void PreFillPool(int count)//预先生成
    {
        var preFillArray=new GameObject[count];
        for (int i = 0; i < count; i++) 
        {
            preFillArray[i]=pool.Get();
        }
        foreach (var item in preFillArray)//释放
        {
            pool.Release(item);
        }
    }
    // 新增方法：从对象池中获取对象
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    // 新增方法：将对象返回到对象池
    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
