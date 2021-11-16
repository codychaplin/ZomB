using UnityEngine;

public class Giftbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Inventory inv = Inventory.instance;
            if (Random.Range(1, 100) <= GameManager.instance.GiftboxAmmoFrequency) // adds weapon ammo
            {
                int rand = Random.Range(1, inv.numWeaponsUnlocked); // random unlocked weapon in inv
                Weapon randWeapon = inv.weaponInventory[rand];
                if (randWeapon != null)
                {
                    int amount = Random.Range(randWeapon.maxAmmo / 8, randWeapon.maxAmmo / 2);
                    randWeapon.AddAmmo(amount);
                }
            }
            else // adds obstacles
            {
                int rand = Random.Range(0, inv.numObstaclesUnlocked); // random unlocked obstacle in inv
                Obstacle randObstacle = inv.obstacleInventory[rand];
                if (randObstacle != null)
                {
                    int amount = Random.Range(1, 10);
                    randObstacle.Add(amount);
                }
            }
            
            Destroy(this.gameObject); // destroy gameobject on pickup
        }
    }
}
