using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, EntityHealth
{
    public float startingHealth = 100f;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerShoot shoot;
    [HideInInspector] public CanvasManager canvasManager;

    public GameObject ragdollPlayerPrefab;

    public GameObject playerModel;
    public float currentHealth;
    private bool dead;

    public AudioSource takeDamageSource;
    public AudioClip damageAudio;

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

        canvasManager.GameEnd();

        playerModel.SetActive(false);

        //Death animation
        GameObject ragdoll = Instantiate(ragdollPlayerPrefab, new Vector3(this.transform.position.x, 1, this.transform.position.z), transform.rotation, this.transform);
        foreach (Transform piece in ragdoll.transform)
        {
            piece.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-2f, 2f), ForceMode.Impulse);
        }
        Destroy(ragdoll, 4f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    
        canvasManager.UpdatePlayerHealth(currentHealth);

        if (currentHealth <= 0f && !dead)
        {
            OnDeath();
        }
        else
        {
            takeDamageSource.PlayOneShot(damageAudio, 0.05f);
        }
    }

    public void GainHealth(float amount)
    {
        if (currentHealth + amount >= 100f)
        {
            currentHealth = 100f;
        }
        else
        {
            currentHealth += amount;
        }
        canvasManager.UpdatePlayerHealth(currentHealth);
    }
}
