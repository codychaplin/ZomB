using UnityEngine;

public class Barrel : MonoBehaviour
{
    public void Explode()
    {
        int mask = 1 << LayerMask.NameToLayer("Enemy");
        int damage = 10;
        int knockback = 5;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, mask); // gets all colliders within radius
        if (colliders.Length > 0)
            foreach (Collider col in colliders)
            {
                Health health = col.GetComponent<Health>();
                if (health != null) // if has health component and not enemyHealth
                {
                    health.TakeDamage(damage); // apply damage
                    Vector3 direction = col.transform.position - transform.position; // get direction from original hit
                    health.OnHit.Invoke(direction, knockback); // apply knockback
                }
            }
    }
}
