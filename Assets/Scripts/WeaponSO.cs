using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;

    public Sprite gunModel;
    public GameObject gun;    
    public float reloadDelay;
    public float firingDelay;
    public int ammo;
    public int clip;

    public GameObject bullet;
    public Color bulletColor;
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletDuration;
}