using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Ammo Settings")]
    public int magazineCapacity = 30;    // how many bullets a magazine can hold
    public int maxReserveAmmo = 90;      // maximum reserve the player can carry for this weapon
    public Sprite GunIcon;

    [Header("Fire Settings")]
    public float fireRate = 10f;
    public float damage = 10f;
    public float reloadTime = 1.5f;
}
