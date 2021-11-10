using UnityEngine;
using UnityEngine.UI;

public class OverheadUI : MonoBehaviour
{
    public Transform target; // player overheadParent
    public GameObject healthbarPrefab; // player overheadParent

    Transform UI;
    Transform cam; // main camera
    GameObject canvas; // worldSpace canvas
    Image healthSlider; // UI image

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        canvas = FindObjectOfType<Canvas>().gameObject;
        UI = Instantiate(healthbarPrefab, canvas.transform).transform;
        healthSlider = UI.GetChild(0).GetComponent<Image>();

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
        float healthPercent = currentHealth / (float)maxHealth;
        healthSlider.fillAmount = healthPercent;

        if (currentHealth <= 0)
            Destroy(UI.gameObject, 1f);
    }
}
