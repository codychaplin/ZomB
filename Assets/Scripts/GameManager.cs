using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion


    public GameObject enemyPrefab;
    public Transform enemiesParent; // parent for spawned enemies
    public Transform[] spawnpoints; // spawnpoints on map
    public List<Weapon> weapons; // list of weapons in game

    int killcount { get; set; }
    readonly int[] weaponUnlocks = new int[] { 0, 2, 4, 6 }; // killcount unlocks

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        Health.onKill += Kill; // subscribe to OnKill delegate
        
        SpawnEnemies(2);
    }

    void Kill()
    {
        killcount++;

        if (killcount == weaponUnlocks[1])
            inventory.AddWeapon(weapons[1], 1); // add shotgun
        else if (killcount == weaponUnlocks[2])
            inventory.AddWeapon(weapons[2], 2); // add M16
        else if (killcount == weaponUnlocks[3])
            inventory.AddWeapon(weapons[3], 3); // add RPG

        Debug.Log("killcount++");
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
            foreach (Transform spawn in spawnpoints)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawn);
                enemy.transform.parent = enemiesParent;
            }
    }
}
