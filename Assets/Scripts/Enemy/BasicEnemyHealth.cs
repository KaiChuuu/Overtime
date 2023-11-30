using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyHealth : MonoBehaviour, EntityHealth
{
    [HideInInspector] public BasicEnemyAI enemyAI;
    public CanvasManager canvasManager;

    public int enemyScore = 0;
    public float enemyHealth = 50f;

    public GameObject model;
    public GameObject ragdoll;
    public AudioSource deathSound;

    private NavMeshAgent agent;
    private BoxCollider agentCollider;

    void Awake()
    {
        deathSound = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agentCollider = GetComponent<BoxCollider>();
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;

        if (enemyHealth <= 0f)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        canvasManager.UpdateKillCount(enemyScore);

        StartCoroutine(DeathEffects());
    }

    IEnumerator DeathEffects()
    {
        //Death animation/move (shows +score value on death)

        enemyAI.DisableAgent();
        agent.enabled = false;
        agentCollider.enabled = false;

        deathSound.Play();

        model.SetActive(false);
        ragdoll.SetActive(true);

        foreach (Transform piece in ragdoll.transform)
        {
            piece.GetComponent<Rigidbody>().AddForce(transform.forward * -3f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(2.5f);

        Destroy(gameObject);
    }
}
