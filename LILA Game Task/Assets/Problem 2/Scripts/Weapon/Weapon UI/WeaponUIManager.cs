using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    public static WeaponUIManager instance;

    [Header("Prefabs")]
    public GameObject primaryWeaponPrefab;
    public GameObject secondaryWeaponPrefab;

    [Header("Parents")]
    public Transform primaryParent;
    public Transform secondaryParent;

    [Header("Buttons")]
    public Button fireButton;
    public Button reloadButton;

    [Header("Slots Limit")]
    public int primaryLimit = 2;
    public int secondaryLimit = 1;

    private Dictionary<WeaponSlot, WeaponUIElementHolder> uiSlots =
        new Dictionary<WeaponSlot, WeaponUIElementHolder>();

    public WeaponSlot currentSlot;
    public WeaponUIElementHolder selectedUI;

    // EVENTS
    public static event Action OnFire;
    public static event Action OnReload;
    public static event Action<WeaponSlot> OnSelectWeapon;

    private void Awake()
    {
        instance = this;
        InitializeUI();
        if (fireButton) fireButton.onClick.AddListener(() => OnFire?.Invoke());
        if (reloadButton) reloadButton.onClick.AddListener(() => OnReload?.Invoke());
    }

    void InitializeUI()
    {
        InitSlot(primaryWeaponPrefab, primaryParent, primaryLimit, WeaponSlot.primaryWeapon1);
        InitSlot(secondaryWeaponPrefab, secondaryParent, secondaryLimit, WeaponSlot.secondaryWeapon);
    }

    void InitSlot(GameObject prefab, Transform parent, int count, WeaponSlot startSlot)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, parent);
            WeaponUIElementHolder holder = obj.GetComponent<WeaponUIElementHolder>();
            WeaponSlot slot = (WeaponSlot)((int)startSlot + i);
            uiSlots.Add(slot, holder);
            // default empty
            UpdateUI(slot, 0, 0, null);
            Button btn = holder.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => SelectSlot(slot));
        }
    }

    void SelectSlot(WeaponSlot slot)
    {
        if (!uiSlots.ContainsKey(slot)) return;

        if (selectedUI != null) selectedUI.SetSelected(false);

        selectedUI = uiSlots[slot];
        selectedUI.SetSelected(true);

        currentSlot = slot;
        OnSelectWeapon?.Invoke(slot);
    }

    // Update UI for a slot using runtime values (current magazine, reserve)
    public void UpdateUI(WeaponSlot slot, int currentMagazine, int reserveAmmo, Sprite icon)
    {
        if (!uiSlots.ContainsKey(slot)) return;
        var ui = uiSlots[slot];

        ui.countText.text = $"{Mathf.Max(0, currentMagazine)} / {Mathf.Max(0, reserveAmmo)}";

        if (ui.weaponIcon != null)
            ui.weaponIcon.sprite = icon;
    }
    public void UpdateUIFromData(WeaponSlot slot, WeaponData data)
    {
        if (!uiSlots.ContainsKey(slot)) return;

        var ui = uiSlots[slot];

        int magazine = data.magazineCapacity;
        int reserve = data.maxReserveAmmo;

        ui.countText.text = $"{magazine} / {reserve}";
        if (selectedUI == null)
        {
            ui.weaponIcon.gameObject.SetActive(true);
            ui.weaponIcon.sprite = data.GunIcon;
            ui.SetSelected(false);
        }
       
    }
}

public enum WeaponSlot
{
    primaryWeapon1,
    primaryWeapon2,
    secondaryWeapon
}
