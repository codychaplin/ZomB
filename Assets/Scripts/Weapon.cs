using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public string weaponName;
    public int ammoCapacity;
    public int damage;
    int currentAmmo;

    public void InitializeWeapon()
    {
        currentAmmo = ammoCapacity; // ammo is set to max capacity
    }

    public void Shoot()
    {
        if (currentAmmo > 0) // if has ammo
        {
            Debug.Log(weaponName + ": Bang");
            
            currentAmmo--; // decrement ammo
        }
        else
            Debug.Log(weaponName + ": Out of ammo");
    }

    public void ShowWeapon(bool show)
    {
        gameObject.SetActive(show); // enable/disable gameObject
    }
}
