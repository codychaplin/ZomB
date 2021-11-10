using UnityEngine;
using UnityEngine.UI;

public class OverheadUI : MonoBehaviour
{
    public Transform target; // player overheadParent
    public GameObject healthbarPrefab; // player overheadParent

    Transform UI;
    Transform cam; // main camera
    GameObject canvas; // worldspace canvas
    Image healthSlider; // UI image

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform; // reference to camera
        canvas = FindObjectOfType<Canvas>().gameObject; // reference to worldspace canvas
        UI = Instantiate(healthbarPrefab, canvas.transform).transform; // healthbar prefab
        healthSlider = UI.GetChild(0).GetComponent<Image>(); // image within healthbar

        // subscribes to player's OnHealthChanged event
        GetComponent<Health>().OnHealthChanged += OnHealthChanged;
    }

    void LateUpdate()
    {
        if (UI != null)
        {
            // update UI position
            UI.position = target.position;
            UI.forward = -cam.forward;
        }
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        float healthPercent = currentHealth / (float)maxHealth; // gets health level as percent
        healthSlider.fillAmount = healthPercent; // update slider fill amount

        if (currentHealth <= 0)
            Destroy(UI.gameObject, 1f); // on death, delete healthbar
    }
}
