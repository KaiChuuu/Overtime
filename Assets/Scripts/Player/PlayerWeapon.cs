using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [HideInInspector] public PlayerManager playerManager;

    public GameObject weaponParent;
    public string bulletSpawnTag = "BulletSpawn";

    private GameObject currentWeapon;
    public BoxCollider bulletSpawn; //Bullet Spawn Zone is CHILD of WeaponParent
    public AudioSource weaponAudio;

    public void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon); //Remove existing weapon
        }

        //Noise effect
 
        currentWeapon = Instantiate(weapon, weaponParent.transform);
 
        bulletSpawn = currentWeapon.GetComponentInChildren<BoxCollider>();
        bulletSpawn.center = new Vector3(bulletSpawn.center.x + weaponParent.transform.localPosition.x,
                                        1,
                                        bulletSpawn.center.z + weaponParent.transform.localPosition.z);
        weaponAudio = currentWeapon.GetComponent<AudioSource>();
        if (!bulletSpawn)
        {
            Debug.Log("No spawn for bullets (error?!?)");
        }
    }

    public void AttachNewGun(WeaponSO newWeapon)
    {
        playerManager.EquipWeapon(newWeapon);
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
