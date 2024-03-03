using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "ScriptableObjects/NewEnemyProfile")]
public class EnemyProfile : ScriptableObject
{
    public EnemyBehavior prefab;
    public AnimationCurve spawnCurve;
    public float spawnCurveDuration;
    public EnemyType enemyType;
}
