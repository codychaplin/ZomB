using UnityEngine;

public class Giftbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Inventory.instance.GiftAmmo(); // adds ammo to random weapon/obstacle
            Destroy(this.gameObject); // destroy gameobject on pickup
        }
    }
}
