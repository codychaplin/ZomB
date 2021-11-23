using UnityEngine;

public class Obstacle : InvObj
{
    public Type type;
    public GameObject prefab;

    public void Init()
    {
        currentAmmo = 20;
    }

    public void Add(int amount)
    {
        currentAmmo += amount;
        Inventory inventory = Inventory.instance;
        if (inventory.currentObstacle == this)
            inventory.onUpdateUI.Invoke(name, currentAmmo, unlimitedAmmo);
    }

    public void Place()
    {
        currentAmmo--;
        Inventory.instance.onUpdateUI.Invoke(name, currentAmmo, unlimitedAmmo);
    }
}

public enum Type { Barricade, Barrel, Turret };