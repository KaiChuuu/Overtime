using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100f;

    [HideInInspector] public CanvasManager canvasManager;

    public float currentHealth;
    private bool dead;

    public void Start()
    {
        canvasManager.SetHealthSlider(startingHealth);
    }

    public void Setup()
    {
        currentHealth = startingHealth;
        dead = false;

        canvasManager.UpdatePlayerHealth(currentHealth);
    }

    void OnDeath()
    {
        dead = true;

        //Explosion Animations?

        gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    
        canvasManager.UpdatePlayerHealth(currentHealth);

        if (currentHealth <= 0f && !dead)
        {
            OnDeath();
        }
    }
}
