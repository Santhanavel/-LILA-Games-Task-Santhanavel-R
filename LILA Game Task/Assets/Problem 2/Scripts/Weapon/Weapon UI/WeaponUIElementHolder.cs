using UnityEngine;
using UnityEngine.UI;

public class WeaponUIElementHolder : MonoBehaviour
{
    public Image selectPanel;
    public Image weaponIcon;
    public Text countText;

    public void SetSelected(bool state)
    {
        if (selectPanel)
            selectPanel.enabled = state;
    }

    public void SetIcon(Sprite icon)
    {
        if (weaponIcon)
            weaponIcon.sprite = icon;
    }
}
