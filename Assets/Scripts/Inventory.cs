using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    // weapon info
    const int NUM_WEAPONS = 4;
    public Weapon[] weaponInventory = new Weapon[NUM_WEAPONS];
    Weapon currentWeapon { get; set; }
    int currentWeaponIndex { get; set; }

    // obstacle info
    public List<Obstacle> obstacleInventory;
    Obstacle currentObstacle { get; set; }
    int currentObstacleIndex { get; set; }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged; // inventory event

    Player player { get; set; }
    GameManager manager { get; set; }

    public int numWeaponsUnlocked { get; private set; }
    public int numObstaclesUnlocked { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        manager = GameManager.instance;

        numWeaponsUnlocked = 0;
        AddWeapon(manager.weapons[0], 0); // adds pistol to inventory

        if (weaponInventory[0] != null)
            SwitchWeapon(0); // switch to weapon in first index

        numObstaclesUnlocked = 2;
        if (obstacleInventory[0] != null)
            SwitchObstacle(0); // switch to obstacle in first index

        foreach (Obstacle ob in obstacleInventory)
            if (ob != null)
                ob.count = 0; // set count to each obstacle in inventory to 0
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0); // weapon slot 0
        if (Input.GetKeyDown(KeyCode.Alpha2) && numWeaponsUnlocked >= 2) // if shotgun is unlocked
            SwitchWeapon(1); // weapon slot 1
        if (Input.GetKeyDown(KeyCode.Alpha3) && numWeaponsUnlocked >= 3) // if M16 is unlocked
            SwitchWeapon(2); // weapon slot 2
        if (Input.GetKeyDown(KeyCode.Alpha4) && numWeaponsUnlocked >= 4) // if RPG is unlocked
            SwitchWeapon(3); // weapon slot 3

        if (Input.GetButtonDown("Fire") || Input.GetButton("Fire")) // shoot current weapon
            currentWeapon.Shoot();

        if (Input.GetKeyDown(KeyCode.R)) // reloads weapon
            currentWeapon.Reload();

        if (Input.GetKeyDown(KeyCode.E)) // places obstacle
            PlaceObstacle(currentObstacle);
    }

    public void AddWeapon(Weapon weapon, int index)
    {
        Weapon newWeapon = Instantiate(weapon, player.weaponParent); // instantiates gun prefab
        numWeaponsUnlocked++; // unlocks new weapon
        weaponInventory[index] = newWeapon; // adds weapon to inventory
        newWeapon.InitializeWeapon(); // initializes weapon properties
        newWeapon.ShowWeapon(false);
    }

    void SwitchWeapon(int index)
    {
        if (currentWeapon != null)
            currentWeapon.ShowWeapon(false); // disables current weapon

        currentWeaponIndex = index; // updates current weapon index
        currentWeapon = weaponInventory[currentWeaponIndex]; // updates current weapon
        currentWeapon.ShowWeapon(true); // enables current weapon

        /*if (onItemChanged != null)
            onItemChanged.Invoke(); // trigger event*/
    }

    void SwitchObstacle(int index)
    {
        currentObstacleIndex = index; // updates current obstacle index
        currentObstacle = obstacleInventory[currentObstacleIndex]; // updates current obstacle

        /*if (onItemChanged != null)
            onItemChanged.Invoke(); // trigger event*/
    }

    void PlaceObstacle(Obstacle obstacle)
    {
        if (currentObstacle != null)
        {
            if (currentObstacle.count > 0)
            {
                currentObstacle.count--;
                Vector3 pos = player.GetTileInfrontOfPlayer();
                Instantiate(obstacle.prefab, pos, Quaternion.identity, manager.obstaclesParent);
            }
            else
                Debug.Log("Out of obstacle");
        }
    }
}
