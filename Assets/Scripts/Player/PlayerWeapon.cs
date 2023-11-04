using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weaponParent;
    public string bulletSpawnTag = "BulletSpawn";

    private GameObject weaponPrefab;
    public BoxCollider bulletSpawn; //Bullet Spawn Zone is CHILD of WeaponParent

    public void EquipWeapon(GameObject weapon)
    {
        if (weaponPrefab)
        {
            Destroy(weaponPrefab); //Remove existing weapon
        }

        //Noise effect

        weaponPrefab = weapon;

        GameObject newWeapon = Instantiate(weaponPrefab, weaponParent.transform);
        bulletSpawn = newWeapon.GetComponentInChildren<BoxCollider>();
        if (!bulletSpawn)
        {
            Debug.Log("No spawn for bullets (error?!?)");
        }
    }
}
