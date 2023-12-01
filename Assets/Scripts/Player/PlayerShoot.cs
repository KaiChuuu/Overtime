using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [HideInInspector] public PlayerWeapon playerWeapon;

    public AudioSource extraWeaponSounds;
    public AudioSource reloadSound;

    public GameObject bulletPrefab;
    [HideInInspector] public CanvasManager canvasManager;
    [HideInInspector] public Transform projectilePool;
    [HideInInspector] public Transform particlePool;
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public float bulletDuration;
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public int maxAmmo;
    [HideInInspector] public int maxClip;
    [HideInInspector] public Color bulletColor;
    [HideInInspector] public string weaponName;  //Used for differentiating different guns

    public bool canShoot;
    public float reloadDelay;
    public float firingDelay;
    private bool canFireNext = true;
    private float firingTimer = 0f;
    public int ammo = 0;
    private float timer = 0f;

    public AudioSource weaponAudio;
    public BoxCollider bulletSpawnLocation;
    [SerializeField]
    private InputActionReference shoot;

    private bool isShooting = false;

    private float windupTime = 1f;
    private float windupTimer = 0f;

    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = transform.parent.GetComponent<Rigidbody>();
        canShoot = false;
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        if(ammo <= 0 && maxAmmo > 0)
        {
            ShootDelay();
        }

        if (!canFireNext)
        {
            firingTimer += Time.deltaTime;
            if(firingTimer > firingDelay)
            {
                firingTimer = 0f;
                canFireNext = true;
            }
        }
        else if (weaponName == "Endbringer")
        {
            if (isShooting && ammo > 0)
            {
                if(windupTimer < windupTime)
                {
                    windupTimer += Time.deltaTime;
                    extraWeaponSounds.pitch += Time.deltaTime * 0.2f;
                    extraWeaponSounds.volume += Time.deltaTime * 0.05f;
                }

                if (windupTimer > windupTime)
                {
                    FireBullet();
                }
            }
            else
            {
                if (windupTimer > 0f)
                {
                    windupTimer -= Time.deltaTime;
                    extraWeaponSounds.pitch -= Time.deltaTime * 0.2f;
                    extraWeaponSounds.volume -= Time.deltaTime * 0.05f;
                }
            }
        }
        else if (isShooting)
        {
            FireBullet();
        }

    }

    void OnEnable()
    {
        shoot.action.performed += ShootBullet;
    }

    void OnDisable()
    {
        shoot.action.performed -= ShootBullet;
    }

    void ShootDelay()
    {
        timer += Time.deltaTime;
        canvasManager.UpdateReloadBar(reloadDelay - timer);

        if(timer > reloadDelay)
        {
            timer = 0f;
            ammo = maxClip;
            maxAmmo -= maxClip;
            canvasManager.UpdateWeaponAmmo(ammo);
            canvasManager.UpdateWeaponMaxAmmo(maxAmmo);
            canvasManager.UpdateReloadBar(reloadDelay);
            canvasManager.DisableReloadBar();
        }
    }

    void ShootBullet(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton() && isShooting)
        {
            isShooting = false;
            return;
        }

        if (context.ReadValueAsButton() && canShoot)
        {
            if (weaponName == "Endbringer")
            {
                extraWeaponSounds.Play();
            }
            isShooting = true;
        }
    }

    void FireBullet()
    {
        if (!canShoot || ammo <= 0 || !canFireNext)
        {
            return;
        }
        canFireNext = false;
        ammo--;
        canvasManager.UpdateWeaponAmmo(ammo);

        if (ammo == 0 && maxAmmo > 0)
        {
            StartCoroutine(ReloadAudio());
            canvasManager.EnableReloadBar();
        }


        //Wrap in 'if' for multiple types of weapons
        switch (weaponName)
        {
            case "Lazer Blaster":
                //Shoot bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center), transform.rotation, projectilePool);
                bullet.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity;
                Bullet bulletComp = bullet.GetComponent<Bullet>();
                UpdateBullet(ref bulletComp);
                break;
            case "Scattergun":
                for (int i = 0; i < 5; i++)
                {
                    float spread = Random.Range(-0.1f, 0.1f);
                    GameObject shell = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center), transform.rotation * Quaternion.Euler(0, spread, 0), projectilePool);
                    shell.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity;
                    Bullet shellComp = shell.GetComponent<Bullet>();
                    UpdateBullet(ref shellComp);
                }
                break;
            case "Discharger":
                float spread2 = Random.Range(-0.1f, 0.1f);
                GameObject bullet2 = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center) + new Vector3(spread2, 0, 0), transform.rotation, projectilePool);
                bullet2.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity;
                Bullet bullet2Comp = bullet2.GetComponent<Bullet>();
                UpdateBullet(ref bullet2Comp);
                break;
            case "Endbringer":
                float spread3 = Random.Range(-2f, 2f);
                GameObject lead = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center), transform.rotation * Quaternion.Euler(0, spread3, 0), projectilePool);
                lead.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity;
                Bullet leadComp = lead.GetComponent<Bullet>();
                UpdateBullet(ref leadComp);
                break;
        }

        weaponAudio.Play();
    }
    
    void UpdateBullet(ref Bullet bullet)
    {
        bullet.speed = bulletSpeed;
        bullet.duration = bulletDuration;
        bullet.damage = bulletDamage;
        bullet.UpdateBulletColor(bulletColor);
        bullet.particlePool = particlePool;
    }
    
    public void ResetReloadBar()
    {
        canvasManager.DisableReloadBar();
        canvasManager.UpdateReloadBar(1);
        timer = 0f;
        canFireNext = true;
    }

    public void ResetExtraAudio()
    {
        if(weaponName == "Endbringer")
        {
            StartCoroutine(LowerAudio());
        }
    }


    IEnumerator LowerAudio()
    {
        for(int i=50; i>=0; i--)
        {
            extraWeaponSounds.volume -= 0.0011f;
            yield return new WaitForSeconds(0.01f);
        }
        windupTimer = 0f;
        extraWeaponSounds.pitch = 0.22f;
        extraWeaponSounds.Stop();
    }

    IEnumerator ReloadAudio()
    {
        float spacing = reloadDelay / 3;
        for(int i=0; i<3; i++)
        {
            reloadSound.Play();
            yield return new WaitForSeconds(spacing);
        }
    }
}
