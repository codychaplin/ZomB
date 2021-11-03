using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public string weaponName;
    public int ammoCapacity;
    public int damage;
    int currentAmmo;

    public Weapon() { } // default constructor

    /*public Weapon(Weapon w) // copy constructor
    {
        weaponPrefab = w.weaponPrefab;
        weaponName = w.weaponName;
        ammoCapacity = w.ammoCapacity;
        damage = w.damage;
        currentAmmo = w.currentAmmo;
        InitializeWeapon();
    }*/

    public void Copy(Weapon w)
    {
        weaponPrefab = w.weaponPrefab;
        weaponName = w.weaponName;
        ammoCapacity = w.ammoCapacity;
        damage = w.damage;
        currentAmmo = w.currentAmmo;
        InitializeWeapon();
    }

    void InitializeWeapon()
    {
        currentAmmo = ammoCapacity;
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            Debug.Log(weaponName + ": Bang");
            currentAmmo--;
        }
        else
            Debug.Log(weaponName + ": Out of ammo");
    }

    public void ShowWeapon(bool show)
    {
        weaponPrefab.SetActive(show);
    }
}
