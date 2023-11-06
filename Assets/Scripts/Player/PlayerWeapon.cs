using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weaponParent;
    public string bulletSpawnTag = "BulletSpawn";

    private GameObject currentWeapon;
    public BoxCollider bulletSpawn; //Bullet Spawn Zone is CHILD of WeaponParent

    public void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon); //Remove existing weapon
        }

        //Noise effect

        currentWeapon = Instantiate(weapon, weaponParent.transform);
        bulletSpawn = currentWeapon.GetComponentInChildren<BoxCollider>();
        if (!bulletSpawn)
        {
            Debug.Log("No spawn for bullets (error?!?)");
        }
    }

    public void DisableGun()
    {
        weaponParent.SetActive(false);
    }

    public void EnableGun()
    {
        weaponParent.SetActive(true);
    }
}
