using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public WeaponSO weaponType;
    public GameObject weapon;

    public bool available = true;
    public float rechargeTime = 5f;
    private float time = 0f;

    public float rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        weapon.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (!available)
        {
            time += Time.deltaTime;
            if (time > rechargeTime)
            {
                time = 0f;
                available = true;
                weapon.SetActive(true);
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
                PlayerWeapon targetWeapon = collider.gameObject.GetComponentInChildren<PlayerWeapon>();
                if (targetWeapon != null)
                {
                    targetWeapon.AttachNewGun(weaponType);
                    available = false;
                    weapon.SetActive(false);
                }
            }
        }
    }
}
