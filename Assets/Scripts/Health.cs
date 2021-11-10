using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isPlayer = false;
    public int maxHealth;
    int currentHealth;

    public event System.Action<int, int> OnHealthChanged;

    public void InitializeHealth()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (OnHealthChanged != null)
            OnHealthChanged(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (!isPlayer)
            GameObject.Destroy(this.gameObject); // deletes enemy from scene
        else
            Debug.Log("You died");
    }
}
