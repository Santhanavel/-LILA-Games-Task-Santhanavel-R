using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Tooltip("WeaponData assets in order: primary1, primary2, secondary (optional)")]
    public List<WeaponData> weapons;
    // Per-slot runtime instance
    class WeaponInstance
    {
        public WeaponData data;
        public int currentMagazine;
        public int reserveAmmo;

        public WeaponInstance(WeaponData d)
        {
            data = d;
            // spawn with a full magazine and full reserve (or customize)
            currentMagazine = d.magazineCapacity;
            reserveAmmo = d.maxReserveAmmo;
        }
    }

    private Dictionary<WeaponSlot, WeaponInstance> weaponSlots =
        new Dictionary<WeaponSlot, WeaponInstance>();

    private WeaponInstance currentInstance;

    private void OnEnable()
    {
        WeaponUIManager.OnSelectWeapon += SelectWeapon;
        WeaponUIManager.OnFire += FireCurrent;
        WeaponUIManager.OnReload += ReloadCurrent;
    }

    private void OnDisable()
    {
        WeaponUIManager.OnSelectWeapon -= SelectWeapon;
        WeaponUIManager.OnFire -= FireCurrent;
        WeaponUIManager.OnReload -= ReloadCurrent;
    }

    private void Start()
    {
        AssignWeapons();
    }
    void AssignWeapons()
    {
        if (weapons == null) return;
        if (weapons.Count >= 1) AddWeapon(WeaponSlot.primaryWeapon1, weapons[0]);
        if (weapons.Count >= 2) AddWeapon(WeaponSlot.primaryWeapon2, weapons[1]);
        if (weapons.Count >= 3) AddWeapon(WeaponSlot.secondaryWeapon, weapons[2]);
    }

    void AddWeapon(WeaponSlot slot, WeaponData data)
    {
        weaponSlots[slot] = new WeaponInstance(data);
        // UI must update based on SO data
        WeaponUIManager.instance.UpdateUIFromData(slot, data);
    }

    void SelectWeapon(WeaponSlot slot)
    {
        if (!weaponSlots.ContainsKey(slot)) return;
        currentInstance = weaponSlots[slot];

        // Update UI with that slot's current values
        WeaponUIManager.instance.UpdateUI(slot, currentInstance.currentMagazine, currentInstance.reserveAmmo, currentInstance.data.GunIcon);
    }

    void FireCurrent()
    {
        if (currentInstance == null) return;
        if (currentInstance.currentMagazine <= 0)
        {
            Debug.Log("No bullets in magazine. Reload!");
            return;
        }

        currentInstance.currentMagazine--;
        // update UI for the currently selected slot
        WeaponSlot slot = GetSlotOf(currentInstance);
        WeaponUIManager.instance.UpdateUI(slot, currentInstance.currentMagazine, currentInstance.reserveAmmo, currentInstance.data.GunIcon);
    }

    void ReloadCurrent()
    {
        if (currentInstance == null) return;

        int capacity = currentInstance.data.magazineCapacity;
        int need = capacity - currentInstance.currentMagazine;           // <- CORRECT: bullets missing in magazine

        if (need <= 0)
        {
            Debug.Log("Magazine already full");
            return;
        }

        if (currentInstance.reserveAmmo <= 0)
        {
            Debug.Log("No reserve ammo to reload");
            return;
        }

        int load = Mathf.Min(need, currentInstance.reserveAmmo);

        currentInstance.currentMagazine += load;
        currentInstance.reserveAmmo -= load;

        // Update UI for this slot
        WeaponSlot slot = GetSlotOf(currentInstance);
        WeaponUIManager.instance.UpdateUI(slot, currentInstance.currentMagazine, currentInstance.reserveAmmo, currentInstance.data.GunIcon);

        Debug.Log($"Reloaded {load} bullets. Magazine: {currentInstance.currentMagazine} / {capacity}, Reserve: {currentInstance.reserveAmmo}");
    }

    WeaponSlot GetSlotOf(WeaponInstance inst)
    {
        foreach (var kv in weaponSlots)
            if (kv.Value == inst) return kv.Key;

        return WeaponSlot.primaryWeapon1; // fallback
    }

}
