using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public WeaponSO weaponType;
    public GameObject itemModel;

    public bool isHealth = false;
    public float healthGain = 100f;

    public bool available = true;
    public float rechargeTime = 5f;
    private float time = 0f;

    public float rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        itemModel.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (!available)
        {
            time += Time.deltaTime;
            if (time > rechargeTime)
            {
                time = 0f;
                available = true;
                itemModel.SetActive(true);
                //Animation 
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            
            if (available)
            {
                if (isHealth)
                {
                    GiveHealth(collider);
                }
                else
                {
                    GiveWeapon(collider);
                }
            }
        }
    }

    void GiveHealth(Collider collider)
    {
        PlayerHealth targetHealth = collider.gameObject.GetComponentInChildren<PlayerHealth>();
        if (targetHealth != null)
        {
            targetHealth.GainHealth(healthGain);
            available = false;
            itemModel.SetActive(false);
        }
    }

    void GiveWeapon(Collider collider)
    {
        PlayerWeapon targetWeapon = collider.gameObject.GetComponentInChildren<PlayerWeapon>();
        if (targetWeapon != null)
        {
            targetWeapon.AttachNewGun(weaponType);
            available = false;
            itemModel.SetActive(false);
        }
    }
}
