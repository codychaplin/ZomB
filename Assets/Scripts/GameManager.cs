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

    [Header("Giftbox Settings")]
    public GameObject giftbox; // giftbox prefab
    public Transform GiftboxesParent;
    [Range(1, 100)]
    public int GiftboxFrequency = 7;
    [Range(1, 100)]
    public int GiftboxAmmoFrequency = 5;
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform enemiesParent; // parent for spawned enemies
    public int enemyCount;
    public bool spawnEnemies;
    public Transform[] spawnpoints; // spawnpoints on map
    [Header("Weapon/Obstacle Settings")]
    public Transform obstaclesParent;
    public List<Weapon> weapons; // list of weapons in game

    public int killcount { get; private set; }
    public readonly int[] weaponUnlocks = new int[] { 0, 2, 4, 6 }; // killcount unlocks

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        Health.onKill += OnKill; // subscribe to OnKill delegate
        
        if (spawnEnemies)
            SpawnEnemies(enemyCount);
    }

    void OnKill()
    {
        killcount++;

        if (killcount == weaponUnlocks[1])
            inventory.AddWeapon(weapons[1], 1); // add shotgun
        else if (killcount == weaponUnlocks[2])
            inventory.AddWeapon(weapons[2], 2); // add M4
        else if (killcount == weaponUnlocks[3])
            inventory.AddWeapon(weapons[3], 3); // add RPG
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
            foreach (Transform spawn in spawnpoints)
                Instantiate(enemyPrefab, spawn.position, Quaternion.identity, enemiesParent);
    }
}
