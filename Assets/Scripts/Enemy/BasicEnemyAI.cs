using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//For ninja enemy
public class BasicEnemyAI : MonoBehaviour, EnemyBaseStats
{
    public float enemyHealth = 50f;
    public float enemyDamage = 10f;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Getting hit
    //collision with player ignored

    public void SetBaseStats(float speed, float damage, float health)
    {
        enemyHealth = health;
        enemyDamage = damage;
        agent.speed = speed;

        agent.enabled = true;
    }
}
