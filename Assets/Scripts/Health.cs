using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isPlayer = false;
    public int maxHealth;
    int currentHealth;

    public event System.Action<int, int> OnHealthChanged; // event
    public delegate void OnKillkDelegate(); // killcount deletegate
    public static OnKillkDelegate OnKill; // static accessor

    public void InitializeHealth()
    {
        currentHealth = maxHealth; // sets health to full
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // add health
        if (currentHealth > maxHealth)
            currentHealth = maxHealth; // clamp to max health

        if (OnHealthChanged != null) // trigger event
            OnHealthChanged(maxHealth, currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // reduce health

        if (currentHealth <= 0)
            Die();

        if (OnHealthChanged != null) // trigger event
            OnHealthChanged(maxHealth, currentHealth);
    }

    void Die()
    {
        if (!isPlayer) // deletes enemy from scene
        {
            if (OnKill != null) // trigger delegate
                OnKill();

            GameObject.Destroy(this.gameObject); // destroy gameobject
        }
        else
            Debug.Log("You died");
    }
}
