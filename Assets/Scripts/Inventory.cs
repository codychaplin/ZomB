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

    InvObj[] inventory = new InvObj[6]; // inventory
    InvObj currentObject { get; set; }
    int currentIndex { get; set; } // index in inventory
    bool[] validIndices = new bool[] { false, false, false, false, false, false };
    const int NUM_WEAPONS = 4;
    const int NUM_OBSTACLES = 2;
    public Weapon currentWeapon { get; private set; }
    public Obstacle currentObstacle { get; private set; }

    public delegate void OnUpdateUI(string name, int ammo, bool hasUnlimitedAmmo);
    public OnUpdateUI onUpdateUI; // inventory event

    Player player { get; set; }
    GameManager manager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // caches singletons
        player = Player.instance;
        manager = GameManager.instance;
        
        AddWeapon(manager.weapons[0], 0); // adds pistol to inventory
        AddObstacle(manager.obstacles[0], 4); // adds barricade to inventory
        AddObstacle(manager.obstacles[1], 5); // adds barrel to inventory
        
        SetSlot(0); // switch to inventory slot 0
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentIndex != 0)
            SetSlot(0); // weapon slot 0
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentIndex != 1 && validIndices[1]) // if shotgun is unlocked
            SetSlot(1); // weapon slot 1
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentIndex != 2 && validIndices[2]) // if M16 is unlocked
            SetSlot(2); // weapon slot 2
        else if (Input.GetKeyDown(KeyCode.Alpha4) && currentIndex != 3 && validIndices[3]) // if RPG is unlocked
            SetSlot(3); // weapon slot 3
        else if (Input.GetKeyDown(KeyCode.Alpha5) && currentIndex != 4 && validIndices[4]) // if barricade is unlocked
            SetSlot(4); // obstacle slot 0
        else if (Input.GetKeyDown(KeyCode.Alpha6) && currentIndex != 5 && validIndices[5]) // if barrel is unlocked
            SetSlot(5); // obstacle slot 1
        else if (Input.GetKeyDown(KeyCode.Q)) // switch to previous slot
            SwitchSlot(-1);
        else if (Input.GetKeyDown(KeyCode.E)) // switch to next slot
            SwitchSlot(1);
            
        if (Input.GetButtonDown("Fire") || Input.GetButton("Fire")) // shoot weapon
            if (currentWeapon != null)
                currentWeapon.Shoot();

        if (Input.GetKeyDown(KeyCode.R)) // reloads weapon
            if (currentWeapon != null)
                currentWeapon.Reload();

        if (Input.GetKeyDown(KeyCode.Space)) // places obstacle
            if (currentObstacle != null)
                PlaceObstacle();
    }

    bool IsTrue()
    {
        Debug.Log("called");
        return true;
    }

    void SwitchSlot(int i)
    {
        if (currentObject != null) // disable previous object
            currentObject.Show(false);

        do
        {
            currentIndex += i; // increment index
            if (currentIndex < 0) // switch to index within range
                currentIndex = inventory.Length - 1;
            else if (currentIndex >= inventory.Length)
                currentIndex = 0;
        }
        while (!validIndices[currentIndex]);
        
        SetCurrent();
    }

    void SetSlot(int i)
    {
        if (currentObject != null) // disable previous object
            currentObject.Show(false);

        currentIndex = i; // update index
        SetCurrent();
    }

    void SetCurrent()
    {
        currentObject = inventory[currentIndex]; // update current object
        currentObject.Show(true); // enable object

        if (currentObject.GetType() == typeof(Weapon)) // if weapon
        {
            currentWeapon = (Weapon)currentObject; // set current weapon
            currentObstacle = null;
        }
        if (currentObject.GetType() == typeof(Obstacle)) // if obstacle
        {
            currentObstacle = (Obstacle)currentObject; // set current obstacle
            currentWeapon = null;
        }

        if (onUpdateUI != null) // trigger event
            onUpdateUI.Invoke(currentObject.name, currentObject.currentAmmo, currentObject.unlimitedAmmo);
    }

    public void AddWeapon(Weapon weapon, int index)
    {
        Weapon newWeapon = Instantiate(weapon, player.weaponParent); // instantiates gun prefab
        validIndices[index] = true; // unlocks new weapon
        inventory[index] = newWeapon; // adds weapon to inventory
        newWeapon.Init(); // initializes ammo
        newWeapon.Show(false);
    }

    void AddObstacle(Obstacle obstacle, int index)
    {
        Obstacle newObstacle = Instantiate(obstacle, player.weaponParent); // instantiates obstacle prefab
        validIndices[index] = true; ; // unlocks new obstacle
        inventory[index] = newObstacle; // adds obstacle to inventory
        newObstacle.Init(); // initializes ammo
        newObstacle.Show(false);
    }

    void PlaceObstacle()
    {
        if (currentObstacle.currentAmmo > 0) // if has ammo
        {
            currentObstacle.Place(); // place obstacle on ground
            Vector3 pos = player.GetTileInfrontOfPlayer();
            Instantiate(currentObstacle.prefab, pos, Quaternion.identity, manager.obstaclesParent);
        }
    }
    
    public void GiftAmmo()
    {
        if (Random.Range(1, 100) <= manager.GiftboxAmmoFrequency) // adds weapon ammo
        {
            int numvalid = 0;
            for (int i = 0; i < NUM_WEAPONS; i++)
                if (validIndices[i]) numvalid++;

            int rand = Random.Range(1, numvalid); // random unlocked weapon in inv
            Weapon randWeapon = (Weapon)inventory[rand];
            if (randWeapon != null)
            {
                // add random amount of ammo
                int amount = Random.Range(randWeapon.maxAmmo / 8, randWeapon.maxAmmo / 2);
                randWeapon.AddAmmo(amount);
            }
        }
        else // adds obstacles
        {
            int numvalid = 0;
            for (int i = inventory.Length - NUM_OBSTACLES; i < inventory.Length; i++)
                if (validIndices[i]) numvalid++;

            int rand = Random.Range(NUM_WEAPONS, NUM_WEAPONS + numvalid); // random unlocked obstacle in inv
            Obstacle randObstacle = (Obstacle)inventory[rand];
            if (randObstacle != null)
            {
                // add random amount of ammo
                int amount = Random.Range(1, 10);
                randObstacle.Add(amount);
            }
        }
    }
}
