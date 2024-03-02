using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemiesPool : MonoBehaviour
{
    public int poolLength;
    public EnemyBehavior prefab;
    private Queue<EnemyBehavior> enemiesQueue = new Queue<EnemyBehavior>();

    void InitializePool()
    {
        EnemyBehavior newEnemy;
        for (int i = 0; i < poolLength; i++)
        {
            newEnemy = Instantiate(prefab, transform);
            newEnemy.gameObject.SetActive(false);
            enemiesQueue.Enqueue(newEnemy);
        }
    }

    public EnemyBehavior GetEnemy()
    {
        return enemiesQueue.Dequeue();
    }

    public void ReturnEnemy(EnemyBehavior _enemy)
    {
        //_enemy.gameObject.SetActive(false);
        enemiesQueue.Enqueue(_enemy);
    }
}
