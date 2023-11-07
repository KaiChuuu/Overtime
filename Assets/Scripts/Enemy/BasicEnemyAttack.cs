using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    public GameObject target;

    public float enemyDamage = 10f;
    public float attackRange = 10f;

    public void Update()
    {
        PerformAttack();
    }

    public void PerformAttack()
    {
        if (target && Vector3.Distance(transform.position, target.transform.position) < attackRange)
        {
            //Start weapon animation, sword/shooting/etc
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint collider = collision.contacts[0];
        string tag = collider.otherCollider.tag;
        switch (tag)
        {
            case "Player":
                DamagePlayer(collider, tag);
                break;
        }
    }

    void DamagePlayer(ContactPoint collider, string tag)
    {
        EntityHealth targetHealth = collider.otherCollider.gameObject.GetComponent<EntityHealth>();
        if (targetHealth != null) //If it hits a box collider from another part of the target
        {
            targetHealth.TakeDamage(enemyDamage);
        }
    }
}
