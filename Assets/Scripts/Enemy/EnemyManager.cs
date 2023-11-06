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
    public int spawnLimit = 20;

    //Pecentage chance of a enemy type being drawn
    //Based on index of enemyTypes
    public List<int> enemyBucket;

    public void SetUp()
    {
        enemyBucket = new List<int>();
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
}
