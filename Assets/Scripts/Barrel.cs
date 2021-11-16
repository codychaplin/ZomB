using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Properties")]
    public int damage = 10;
    public int knockback = 5;

    public void Explode()
    {
        int mask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, mask); // gets all colliders within radius
        if (colliders.Length > 0)
            foreach (Collider col in colliders)
                if (col.TryGetComponent(out Health health)) // if has health component
                {
                    health.TakeDamage(damage); // apply damage
                    Vector3 direction = col.transform.position - transform.position; // get direction from original hit
                    health.OnHit.Invoke(direction, knockback); // apply knockback
                }
    }
}
