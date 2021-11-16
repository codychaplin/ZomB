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
    [Range(0,5)]
    public int startingWave = 0;
    public bool spawnEnemies;
    public Transform[] spawnpoints; // spawnpoints on map
    [Header("Weapon/Obstacle Settings")]
    public Transform obstaclesParent;
    public List<Weapon> weapons; // list of weapons in game

    public bool isPaused { get; private set; }

    int wave { get; set; }
    int enemyCount { get; set; }
    public int killcount { get; private set; }
    int[] enemiesInWave = new int[] { 4, 8, 16, 32, 48, 64 };
    public readonly int[] weaponUnlocks = new int[] { 0, 2, 4, 6 }; // killcount unlocks

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        inventory = Inventory.instance;
        Health.onKill += OnKill; // subscribe to OnKill delegate

        wave = startingWave;
        enemyCount = 0;
        StartCoroutine(SpawnWave(startingWave)); // spawns enemies in waves using 
        wave++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    void PauseGame()
    {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
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

    IEnumerator SpawnWave(int wave)
    {
        while (enemyCount < enemiesInWave[wave])
        {
            foreach (Transform spawn in spawnpoints)
            {
                Instantiate(enemyPrefab, spawn.position, Quaternion.identity, enemiesParent);
                enemyCount++;
                if (enemyCount >= enemiesInWave[wave]) break;

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
