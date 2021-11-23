using UnityEngine;

public class Weapon : InvObj
{
    public int maxAmmo;
    public int damage;
    public int knockback;
    public float fireRate;
    public bool hasBlastRadius = false;

    [Header("References")]
    public Transform muzzle;

    float nextShot;

    public void Init()
    {
        currentAmmo = maxAmmo; // ammo is set to max capacity
    }

    public void Shoot()
    {
        if (Time.time >= nextShot)
        {
            if (currentAmmo > 0) // if has ammo
            {
                Ray ray = new Ray(muzzle.position, muzzle.forward); // ray that shoots from muzzle
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    Health enemyHealth = hit.collider.GetComponent<Health>();
                    if (enemyHealth != null) // if collider has health component
                    {
                        enemyHealth.TakeDamage(damage); // apply damage
                        enemyHealth.OnHit.Invoke(ray.direction, knockback); // apply knockback
                    }

                    if (hasBlastRadius)
                    {
                        int mask = 1 << LayerMask.NameToLayer("Enemy");
                        Collider[] colliders = Physics.OverlapSphere(hit.point, 3f, mask); // gets all colliders within radius
                        if (colliders.Length > 0)
                            foreach (Collider col in colliders)
                            {
                                Health health = col.GetComponent<Health>();
                                if (health != null && health != enemyHealth) // if has health component and not enemyHealth
                                {
                                    health.TakeDamage(damage); // apply damage
                                    Vector3 direction = col.transform.position - hit.point; // get direction from original hit
                                    health.OnHit.Invoke(direction, knockback); // apply knockback
                                }
                            }
                    }
                }

                //Debug.Log(weaponName + ": Bang");
                nextShot = Time.time + fireRate;
                if (!unlimitedAmmo)
                    currentAmmo--; // decrement ammo

                Inventory.instance.onUpdateUI.Invoke(name, currentAmmo, unlimitedAmmo);
            }
            else
                Debug.Log(name + ": Out of ammo");
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");
        currentAmmo = maxAmmo;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;

        Inventory inventory = Inventory.instance;
        if (inventory.currentWeapon == this)
            inventory.onUpdateUI.Invoke(name, currentAmmo, unlimitedAmmo);
    }
}
