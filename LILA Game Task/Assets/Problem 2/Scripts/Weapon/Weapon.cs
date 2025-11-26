using UnityEngine;

public class Weapon
{
    public WeaponData Data { get; private set; }

    public int CurrentAmmo { get; private set; }
    public int ReserveAmmo { get; private set; }
    private float lastFireTime;

    public Weapon(WeaponData data)
    {
        Data = data;
        CurrentAmmo = data.magazineSize;
        ReserveAmmo = data.maxReserveAmmo;
    }

    public void Fire()
    {
        if (!CanFire()) return;

        CurrentAmmo--;
        lastFireTime = Time.time;
    }

    bool CanFire()
    {
        if (CurrentAmmo <= 0) return false;
        return Time.time >= lastFireTime + Data.fireRate;
    }
}
