using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public string weaponName;
    public int ammoCapacity;
    public int damage;
    public int knockback;
    public float fireRate;
    float nextShot;

    int currentAmmo;

    public void InitializeWeapon()
    {
        currentAmmo = ammoCapacity; // ammo is set to max capacity
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
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                        enemyHealth.OnHit.Invoke(ray.direction, knockback);
                    }
                }

                //Debug.Log(weaponName + ": Bang");
                nextShot = Time.time + fireRate;
                currentAmmo--; // decrement ammo
            }
            else
                Debug.Log(weaponName + ": Out of ammo");
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");
        currentAmmo = ammoCapacity;
    }

    public void ShowWeapon(bool show)
    {
        gameObject.SetActive(show); // enable/disable gameObject
    }
}
