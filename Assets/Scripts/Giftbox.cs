using UnityEngine;

public class Giftbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Inventory inv = Inventory.instance;
            if (Random.value >= 0.3f) // 70% chance of adding weapon ammo
            {
                int rand = Random.Range(0, inv.numWeaponsUnlocked - 1); // random unlocked weapon in inv
                Weapon randWeapon = inv.weaponInventory[rand];
                if (randWeapon != null)
                {
                    int amount = Random.Range(randWeapon.maxAmmo / 8, randWeapon.maxAmmo / 2);
                    randWeapon.AddAmmo(amount);
                }
                else
                    Debug.Log("rand weapon is null");
            }
            else // 30% chance of adding obstacles
            {
                int rand = Random.Range(0, inv.numObstaclesUnlocked - 1); // random unlocked obstacle in inv
                Obstacle randObstacle = inv.obstacleInventory[rand];
                if (randObstacle != null)
                {
                    int amount = Random.Range(1, 10);
                    randObstacle.Add(amount);
                }
                else
                    Debug.Log("rand obstacle is null");
            }
            
            Destroy(this.gameObject); // destroy gameobject on pickup
        }
    }
}
