using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//For ninja enemy
public class BasicEnemyAI : MonoBehaviour, EnemyBaseStats
{
    public BasicEnemyAttack basicEnemyAttack;
    public BasicEnemyHealth basicEnemyHealth;

    private float baseSpeed = 5f;
    private float slowSpeed = 3f;
    public float enemyHealth = 50f;
    private GameObject target;

    private NavMeshAgent agent;
    private Animator animator;

    private float time = 0f;
    public float survivalLimit = 60f; //failsafe for looping the same enemies

    private bool enableAgent = false;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > survivalLimit)
        {
            //Upgrade enemy
        }


        if (enableAgent)
        {
            UpdatePosition();
        }
    }

    public void SetBaseStats(float speed, float slow, float damage, float health, float attackingRange)
    {
        basicEnemyHealth.enemyHealth = health;
        basicEnemyAttack.enemyDamage = damage;
        basicEnemyAttack.attackRange = attackingRange;
        agent.speed = speed;

        baseSpeed = speed;
        slowSpeed = slow;

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
        basicEnemyAttack.animator = animator;

        target = gameTarget;
        basicEnemyAttack.target = gameTarget;
    }

    void UpdatePosition()
    {
        agent.destination = target.transform.position;
    }

    public void SlowdownSpeed()
    {
        agent.speed = slowSpeed;
    }

    public void SetDefaultSpeed()
    {
        agent.speed = baseSpeed;
    }
}