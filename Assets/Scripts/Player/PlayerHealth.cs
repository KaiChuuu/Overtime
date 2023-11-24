using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, EntityHealth
{
    public float startingHealth = 100f;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerShoot shoot;
    [HideInInspector] public CanvasManager canvasManager;

    public GameObject playerModel;
    public float currentHealth;
    private bool dead;

    public void Setup()
    {
        currentHealth = startingHealth;
        dead = false;

        canvasManager.SetHealthSlider(startingHealth);
        canvasManager.UpdatePlayerHealth(currentHealth);

        playerModel.SetActive(true);
    }

    void OnDeath()
    {
        dead = true;

        //Death animation

        canvasManager.GameEnd();

        playerModel.SetActive(false);
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
