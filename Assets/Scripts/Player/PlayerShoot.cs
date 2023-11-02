using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public BoxCollider bulletSpawnLocation;

    [SerializeField]
    private InputActionReference shoot;

    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        shoot.action.performed += ShootBullet;
    }

    private void OnDisable()
    {
        shoot.action.performed -= ShootBullet;
    }

    private void ShootBullet(InputAction.CallbackContext context)
    {
        //Shoot bullet
        Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnLocation.center), transform.rotation);
    }
}
