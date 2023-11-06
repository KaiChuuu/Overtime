using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagers : MonoBehaviour
{
    public GameObject enemyParent;
    public EnemyManager enemies;
    public BoxCollider[] enemySpawners;
    private int activeSpawners = 2;          //Based as index range for enemySpawner array

    public bool canSpawn = false;
    public float startingSpawnDelay = 3f;    //Starting amount of time between each enemy spawns
    public float minSpawnDelay = 1f;         //Minimum amount of time between each enemy spawns
    public float spawnDelayDecayTime = 0.1f; //How much time gets reduced per difficulty increase
    private float spawnDelay;

    private float timer = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        spawnDelay = startingSpawnDelay;
        enemies.SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;

        if(timer > spawnDelay)
        {
            timer = 0f;
            SpawnEnemy(enemies.GetEnemy());
        }
    }

    //Interval when increasing map size
    //Must ensure activeSpawners doesn't go past
    //Enemyspawner array count

    Vector3 RandomSpawnLocation()
    {
        //Select random spawner
        int spawner = UnityEngine.Random.Range(0, activeSpawners);

        //Select random location within spawner
        Vector3 spawnRange = enemySpawners[spawner].size/2;

        float randomXPosition = UnityEngine.Random.Range(-spawnRange.x, spawnRange.x);
        float randomZPosition = UnityEngine.Random.Range(-spawnRange.z, spawnRange.z);

        Vector3 spawnLocation = enemySpawners[spawner].transform.position;
        spawnLocation.x += randomXPosition;
        spawnLocation.z += randomZPosition;

        return spawnLocation;
    }

    void SpawnEnemy(EnemySO enemySO)
    {
        //randomly pick spawner
        Vector3 spawn = RandomSpawnLocation();

        //call enemy manager
        GameObject newEnemy = Instantiate(enemySO.enemy, enemyParent.transform);
        newEnemy.transform.position = spawn;

        //open enemy type, and update values depending on difficulty
    }

    public void IncreaseDifficulty(int difficulty)
    {
        enemies.IncreaseDifficulty(difficulty);

        //Reduce spawn time
        if(spawnDelay > minSpawnDelay)
        {
            spawnDelay -= spawnDelayDecayTime;
        }
    }

    public void EnableSpawning()
    {
        canSpawn = true;
    }

    public void DisableSpawning()
    {
        canSpawn = false;
        timer = 0;
    }

    public void ResetSpawningManager()
    {
        DisableSpawning();

        //Rest spawn information
        spawnDelay = startingSpawnDelay;
        activeSpawners = 2;
        enemies.ResetGame();
    }
}
