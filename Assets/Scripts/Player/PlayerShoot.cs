using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    [HideInInspector] public CanvasManager canvasManager;
    [HideInInspector] public Transform projectilePool;
    [HideInInspector] public Transform particlePool;
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public float bulletDuration;
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public int maxAmmo;
    [HideInInspector] public Color bulletColor;

    public string weaponName;  //Used for differentiating different guns

    public bool canShoot;
    public float reloadDelay;
    public int ammo = 0;
    private float timer = 0f;

    public AudioSource weaponAudio;
    public BoxCollider bulletSpawnLocation;
    [SerializeField]
    private InputActionReference shoot;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = false;
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        if(ammo <= 0)
        {
            ShootDelay();
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
            ammo = maxAmmo;
            canvasManager.UpdateWeaponAmmo(ammo);
            canvasManager.UpdateReloadBar(reloadDelay);
        }
    }

    void ShootBullet(InputAction.CallbackContext context)
    {
        if (!canShoot || ammo <= 0)
        {
            return;
        }
        ammo--;
        canvasManager.UpdateWeaponAmmo(ammo);


        //Wrap in 'if' for multiple types of weapons

        //Shoot bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center), transform.rotation, projectilePool);
        Bullet bulletComp = bullet.GetComponent<Bullet>();
        bulletComp.speed = bulletSpeed;
        bulletComp.duration = bulletDuration;
        bulletComp.damage = bulletDamage;
        bulletComp.UpdateBulletColor(bulletColor);
        bulletComp.particlePool = particlePool;

        weaponAudio.Play();
    }
}
