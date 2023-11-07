using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//For ninja enemy
public class BasicEnemyAI : MonoBehaviour, EnemyBaseStats
{
    public BasicEnemyAttack basicEnemyAttack;
    public BasicEnemyHealth basicEnemyHealth;

    public float enemyHealth = 50f;
    private GameObject target;

    private NavMeshAgent agent;

    private bool enableAgent = false;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableAgent)
        {
            UpdatePosition();
        }
    }

    public void SetBaseStats(float speed, float damage, float health, float attackingRange)
    {
        basicEnemyHealth.enemyHealth = health;
        basicEnemyAttack.enemyDamage = damage;
        basicEnemyAttack.attackRange = attackingRange;
        agent.speed = speed;

        enableAgent = true;
        agent.enabled = true;
    }

    public void SetEnemyComponents(int scorePoints, ref CanvasManager canvas)
    {
        basicEnemyHealth.enemyScore = scorePoints;
        basicEnemyHealth.canvasManager = canvas;
    }

    public void SetEnemyTarget(ref GameObject gameTarget)
    {
        target = gameTarget;
        basicEnemyAttack.target = gameTarget;
    }

    void UpdatePosition()
    {
        agent.destination = target.transform.position;
    }
}