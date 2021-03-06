using UnityEngine;
using UnityEngine.UI;

public class OverheadUI : MonoBehaviour
{
    [Header("References")]
    public Transform target; // player overheadParent
    public GameObject healthbarPrefab; // healthbar
    public GameObject WeaponInfoPrefab; // weapon/ammo info
    public Canvas canvas; // worldspace canvas

    Transform healthbar;
    Transform weaponInfo;
    Transform cam; // main camera
    Image healthSlider; // UI image
    Text objectText; // weapon info

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform; // reference to camera
        healthbar = Instantiate(healthbarPrefab, canvas.transform).transform; // healthbar prefab
        healthSlider = healthbar.GetChild(0).GetComponent<Image>(); // image within healthbar
        weaponInfo = Instantiate(WeaponInfoPrefab, canvas.transform).transform; // healthbar prefab
        objectText = weaponInfo.GetComponent<Text>();

        // subscribes to player's OnHealthChanged event
        GetComponent<Health>().OnHealthChanged.AddListener(OnHealthChanged);
        Inventory.instance.onUpdateUI += OnItemChanged;
    }

    void LateUpdate()
    {
        if (healthbar != null)
        {
            // update UI position
            healthbar.position = target.position;
            healthbar.forward = -cam.forward;
        }

        if (weaponInfo != null)
        {
            // update UI position
            weaponInfo.position = target.position + Vector3.up;
            weaponInfo.forward = cam.forward;
        }
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        float healthPercent = currentHealth / (float)maxHealth; // gets health level as percent
        healthSlider.fillAmount = healthPercent; // update slider fill amount

        if (currentHealth <= 0 && healthbar != null)
            Destroy(healthbar.gameObject, 1f); // on death, delete healthbar
    }

    void OnItemChanged(string name, int ammo, bool hasUnlimitedAmmo)
    {
        objectText.text = hasUnlimitedAmmo ? name : name + ": " + ammo;
    }
}
