using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    [Tooltip("Enemy difficulty increases in array")]
    public EnemySO[] enemyTypes;
    //Index for adding enemyTypes to bucket
    private int nextEnemyType = 1;

    public int gameDifficulty = 0;

    private GameObject player;
    private CanvasManager canvasManager;

    //Pecentage chance of a enemy type being drawn
    //Based on index of enemyTypes
    [Tooltip("Pecentage chance of a enemy type being drawn.\nValues are based on index of enemyTypes.")]
    public List<int> enemyBucket;

    public void SetUp(ref GameObject gamePlayer, ref CanvasManager canvas)
    {
        enemyBucket = new List<int>();
        player = gamePlayer;
        canvasManager = canvas;

        ResetGame();
    }

    public void ResetGame()
    {
        gameDifficulty = 0;
        nextEnemyType = 0;
        ResetBucket();
    }

    public void ResetBucket()
    {
        enemyBucket.Clear();
        for(int i=0; i<enemyTypes[0].bucketAmount; i++)
        {
            enemyBucket.Add(0);
        }
    }

    public void IncreaseDifficulty(int difficulty)
    {
        gameDifficulty = difficulty;

        UpdateEnemySpawnChances();
    }

    public void UpdateEnemySpawnChances()
    {
        //Checks if we can spawn a new type of enemy based on their difficulty level and our current game difficulty
        if (nextEnemyType < enemyTypes.Length && enemyTypes[nextEnemyType].enemyDifficulty <= gameDifficulty)
        {
            //Add in new enemy type into bucket
            for (int i = 0; i < enemyTypes[nextEnemyType].bucketAmount; i++)
            {
                enemyBucket.Add(nextEnemyType);
            }

            nextEnemyType++;
        }
    }

    public EnemySO GetEnemy()
    {
        //Randomizer
        int bucketChoice = UnityEngine.Random.Range(0, enemyBucket.Count);

        return enemyTypes[enemyBucket[bucketChoice]];
    }

    public void UpdateEnemyStats(EnemySO enemySO, ref GameObject enemy)
    {
        //Calculation for increasing stats

        float speed = enemySO.speed;
        float damage = enemySO.damage;
        float health = enemySO.health;

        if (enemySO.enemyDifficulty + enemySO.difficultyCap > gameDifficulty)
        {
            speed += gameDifficulty * enemySO.speedScale;
            damage += gameDifficulty * enemySO.damageScale;
            health += gameDifficulty * enemySO.healthScale;
        }
        else
        {
            //Game difficulty is past max
            speed += enemySO.difficultyCap * enemySO.speedScale;
            damage += enemySO.difficultyCap * enemySO.damageScale;
            health += enemySO.difficultyCap * enemySO.healthScale;
        }

        //Update enemy
        enemy.GetComponent<EnemyBaseStats>().SetEnemyTarget(ref player);
        enemy.GetComponent<EnemyBaseStats>().SetEnemyComponents(enemySO.scorePoints, ref canvasManager);
        enemy.GetComponent<EnemyBaseStats>().SetBaseStats(speed, damage, health, enemySO.attackingRange);
    }
}
