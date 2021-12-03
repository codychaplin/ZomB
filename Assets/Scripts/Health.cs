using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Characteristics")]
    public bool isPlayer = false;
    public bool isObstacle = false;
    public bool isInvincible = false; // debug
    public int maxHealth;

    int currentHealth;
    GameManager manager;
    
    [HideInInspector]
    public UnityEvent<int, int> OnHealthChanged; // UI event
    [HideInInspector]
    public UnityEvent<Vector3, float> OnHit; // knockback event

    public delegate void OnKillDelegate();
    public static OnKillDelegate onKill; // onKill static delegate
    public delegate void OnPlayerDeathDelegate();
    public static OnPlayerDeathDelegate onPlayerDeath; // onPlayerDeath static delegate

    private void Start()
    {
        currentHealth = maxHealth; // sets health to full
        manager = GameManager.instance;
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // add health

        if (currentHealth > maxHealth)
            currentHealth = maxHealth; // clamp to max health

        if (OnHealthChanged != null && isPlayer) // trigger event
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

        if (OnHealthChanged != null && isPlayer) // trigger event
            OnHealthChanged.Invoke(maxHealth, currentHealth);
    }

    void Die()
    {
        if (!isPlayer) // deletes enemy from scene
        {
            if (onKill != null && !isObstacle) // trigger killcount delegate
            {
                // spawn giftbox on death
                if (manager.killcount >= manager.weaponUnlocks[1] && Random.Range(1, 100) <= manager.GiftboxFrequency) // chance of spawning
                    Instantiate(manager.giftbox, transform.position, Quaternion.identity, manager.GiftboxesParent);

                onKill.Invoke();
            }

            if (TryGetComponent(out Barrel barrel))
            {
                barrel.Explode();
            }

            GameObject.Destroy(this.gameObject); // destroy gameobject
        }
        else
        {
            if (isInvincible)
            {
                Heal(maxHealth);
                if (OnHealthChanged != null) // trigger event
                    OnHealthChanged.Invoke(maxHealth, currentHealth);
            }
            else
                onPlayerDeath.Invoke();
        }
    }
}
