using UnityEngine;
using UnityEngine.Pool;
[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        //��ʼ�������
        pool = new ObjectPool<GameObject>(
          createFunc: ()=>Instantiate(objPrefab,transform),//��������������Ϊ��ǰ����
          actionOnGet:(obj)=>obj.SetActive(value:true),//��ȡ�����ã�
          actionOnRelease:(obj)=>obj.SetActive(value:false),//����
          actionOnDestroy:(obj)=>Destroy(obj),//����
          collectionCheck:false,
          defaultCapacity:10,//Ĭ�ϴ�С
          maxSize:20   //�������        
            );
        PreFillPool(7);
    }
    private void PreFillPool(int count)//Ԥ������
    {
        var preFillArray=new GameObject[count];
        for (int i = 0; i < count; i++) 
        {
            preFillArray[i]=pool.Get();
        }
        foreach (var item in preFillArray)//�ͷ�
        {
            pool.Release(item);
        }
    }
    // �����������Ӷ�����л�ȡ����
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    // ���������������󷵻ص������
    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
