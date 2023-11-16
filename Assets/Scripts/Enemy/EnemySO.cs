using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    public GameObject enemy;
    public int scorePoints;

    public float speed;
    public float slowSpeed;
    public float damage;
    public float health;

    //Scaling increase factors per difficulty level 
    public float speedScale;
    public float damageScale;
    public float healthScale;

    public float attackingRange;

    [Tooltip("Amount of percentage added to the bucket affecting the chance of spawn")]
    public int bucketAmount;

    [Tooltip("Current enemy difficulty from 0 to ~")]
    public int enemyDifficulty;

    [Tooltip("Number of times enemy difficulty increases")]
    public int difficultyCap;
}
