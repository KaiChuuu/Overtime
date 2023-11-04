using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;

    public GameObject gun;    
    public float reloadDelay;
    public int ammo;

    public GameObject bullet;
    public Color bulletColor;
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletDuration;
}