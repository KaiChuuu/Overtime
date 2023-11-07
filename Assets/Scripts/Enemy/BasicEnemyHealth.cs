using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHealth : MonoBehaviour, EntityHealth
{
    public CanvasManager canvasManager;

    public int enemyScore = 0;
    public float enemyHealth = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;

        //Display hp bar above

        if (enemyHealth <= 0f)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        //Death animation/move (shows +score value on death)

        canvasManager.UpdateKillCount(enemyScore);

        Destroy(gameObject);
    }
}
