using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy enemy;


    private void Awake()
    {
        

        enemy = GetComponent<Enemy>();


    }

}
