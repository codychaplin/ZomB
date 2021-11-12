using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public bool isPlayer = false;
    public bool isObstacle = false;
    public int maxHealth;
    int currentHealth;
    
    [HideInInspector]
    public UnityEvent<int, int> OnHealthChanged; // UI event
    [HideInInspector]
    public UnityEvent<Vector3, float> OnHit; // knockback event

    public delegate void OnKillDelegate();
    public static OnKillDelegate onKill; // onKill static delegate

    private void Start()
    {
        currentHealth = maxHealth; // sets health to full
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // add health

        if (currentHealth > maxHealth)
            currentHealth = maxHealth; // clamp to max health

        if (OnHealthChanged != null) // trigger event
            OnHealthChanged.Invoke(maxHealth, currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // reduce health

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        if (OnHealthChanged != null) // trigger event
            OnHealthChanged.Invoke(maxHealth, currentHealth);
    }

    void Die()
    {
        if (!isPlayer) // deletes enemy from scene
        {
            if (onKill != null && !isObstacle) // trigger killcount delegate
                onKill.Invoke();

            GameObject.Destroy(this.gameObject); // destroy gameobject
        }
        else
            Debug.Log("You died");
    }
}
