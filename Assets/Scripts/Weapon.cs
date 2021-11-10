using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public string weaponName;
    public int ammoCapacity;
    public int damage;

    public GameObject sourcePrefab { get; set; }
    int currentAmmo;

    public void InitializeWeapon()
    {
        currentAmmo = ammoCapacity; // ammo is set to max capacity
    }

    public void Shoot()
    {
        if (currentAmmo > 0) // if has ammo
        {
            Ray ray = new Ray(muzzle.position, muzzle.forward); // ray that shoots from muzzle
            int mask = 1 << LayerMask.NameToLayer("Enemy"); // layerMask of enemy
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mask))
            {
                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                    enemyHealth.TakeDamage(damage);
            }
            
            //Debug.Log(weaponName + ": Bang");
            currentAmmo--; // decrement ammo
        }
        else
            Debug.Log(weaponName + ": Out of ammo");
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
