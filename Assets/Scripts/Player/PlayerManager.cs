using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    private CanvasManager canvas;
    public Camera gameCamera;
    public Transform projectilePool;
    public Transform particlePool;

    public GameObject player;
    public WeaponSO defaultWeapon;
    public Transform spawnPoint;

    private PlayerHealth health;
    private PlayerMovement movement;
    private PlayerShoot shoot;
    private PlayerAim aim;
    private PlayerWeapon weapon;

    private Animator animator;

    public void Setup(ref CanvasManager canvasManager)
    {   
        canvas = canvasManager;

        //Grab Player Scripts
        health = player.GetComponent<PlayerHealth>();
        movement = player.GetComponent<PlayerMovement>();
        shoot = player.GetComponentInChildren<PlayerShoot>();
        aim = player.GetComponentInChildren<PlayerAim>();
        weapon = player.GetComponentInChildren<PlayerWeapon>();
        animator = player.GetComponent<Animator>();

        //Pass Values
        health.canvasManager = canvas;
        shoot.canvasManager = canvas;
        aim.canvasManager = canvas;
        aim.gameCamera = gameCamera;
        movement.animator = animator;
        weapon.playerManager = this;

        ResetPlayer();
    }

    public void EquipWeapon(WeaponSO weaponType)
    {
        //Bullet
        shoot.projectilePool = projectilePool;
        shoot.particlePool = particlePool;
        shoot.bulletPrefab = weaponType.bullet;
        shoot.bulletSpeed = weaponType.bulletSpeed;
        shoot.bulletDuration = weaponType.bulletDuration;
        shoot.bulletDamage = weaponType.bulletDamage;
        shoot.bulletColor = weaponType.bulletColor;
        shoot.reloadDelay = weaponType.reloadDelay;
        shoot.firingDelay = weaponType.firingDelay;
        shoot.maxAmmo = weaponType.ammo;
        shoot.maxClip = weaponType.clip;
        shoot.ammo = weaponType.clip;
        shoot.weaponName = weaponType.weaponName;
        shoot.ResetReloadBar();

        //Weapon
        weapon.EquipWeapon(weaponType.gun);

        shoot.bulletSpawnLocation = weapon.bulletSpawn; //Updated after EquipWeapon()
        shoot.weaponAudio = weapon.weaponAudio;

        //Canvas Weapon UI
        canvas.SetWeaponUI(weaponType.weaponName, weaponType.clip, weaponType.ammo, weaponType.reloadDelay, weaponType.gunModel);
    }

    public void ResetPlayer()
    {
        //Disable shooting while equip weapon occurs
        shoot.canShoot = false;

        //Default weapon kit
        EquipWeapon(defaultWeapon);
        weapon.DisableGun();

        movement.SetPosition(spawnPoint.position);
        movement.ResetPhysics();

        health.Setup();

        //Other
    }

    public void StartGame()
    {
        shoot.canShoot = true;
        movement.canMove = true;
        weapon.EnableGun();
    }
    
    public void EndGame()
    {
        shoot.canShoot = false;
        movement.canMove = false;
    }
}
