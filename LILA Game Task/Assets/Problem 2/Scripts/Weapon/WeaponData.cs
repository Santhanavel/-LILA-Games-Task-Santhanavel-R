using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public WeaponCategory category;

    [Header("Ammo Settings")]
    public int magazineSize;
    public int maxReserveAmmo;

    [Header("Fire Settings")]
    public float fireRate;
    public float damage;
    public float reloadTime;

    [Header("Effects")]
    public GameObject muzzleFlashPrefab;
    public AudioClip fireSFX;
}
