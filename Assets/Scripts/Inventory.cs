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

    const int INVENTORY_SIZE = 5;
    public Weapon[] inventory = new Weapon[INVENTORY_SIZE];
    Weapon currentWeapon;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged; // inventory event

    Player player;
    public int currentWeaponIndex { get; set; }

    bool[] weaponsUnlocked = { false, false, false, false }; // pistol, shotgun, M16, RPG

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;

        AddWeapon(GameManager.instance.weapons[0], 0); // adds pistol to inventory
        SwitchWeapon(0); // switch to weapon in first index
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0); // weapon slot 0
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponsUnlocked[1]) // if shotgun is unlocked
            SwitchWeapon(1); // weapon slot 1
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsUnlocked[2]) // if M16 is unlocked
            SwitchWeapon(2); // weapon slot 2
        if (Input.GetKeyDown(KeyCode.Alpha4) && weaponsUnlocked[3]) // if RPG is unlocked
            SwitchWeapon(3); // weapon slot 3

        if (Input.GetButtonDown("Fire"))
            inventory[currentWeaponIndex].Shoot();
    }

    void AddWeapon(Weapon weapon, int index)
    {
        weaponsUnlocked[index] = true; // unlocks weapon at index
        inventory[index] = weapon; // adds weapon to inventory
        weapon.InitializeWeapon(); // initializes weapon properties
        Instantiate(weapon.gameObject, player.weaponParent); // instantiates gun prefab
    }

    void SwitchWeapon(int index)
    {
        if (currentWeapon)
            currentWeapon.ShowWeapon(false); // disables current weapon

        currentWeaponIndex = index; // updates current weapon index
        currentWeapon = inventory[currentWeaponIndex]; // updates current weapon
        currentWeapon.ShowWeapon(true); // enables current weapon

        /*if (onItemChanged != null)
            onItemChanged.Invoke(); // trigger event*/
    }
}
