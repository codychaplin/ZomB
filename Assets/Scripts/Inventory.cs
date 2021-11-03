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
        
        Weapon pistol = ScriptableObject.CreateInstance<Weapon>();
        pistol.Copy(GameManager.instance.weapons[0]);
        AddWeapon(pistol, 0); // adds pistol to inventory
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponsUnlocked[1])
            SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsUnlocked[2])
            SwitchWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && weaponsUnlocked[3])
            SwitchWeapon(3);

        if (Input.GetButtonDown("Fire"))
            inventory[currentWeaponIndex].Shoot();
    }

    void AddWeapon(Weapon weapon, int index)
    {
        weaponsUnlocked[index] = true;
        inventory[index] = weapon;
        weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, player.weaponParent);
    }

    void SwitchWeapon(int index)
    {
        if (currentWeapon)
            currentWeapon.ShowWeapon(false);

        currentWeaponIndex = index;
        currentWeapon = inventory[currentWeaponIndex];
        currentWeapon.ShowWeapon(true);

        /*if (onItemChanged != null)
            onItemChanged.Invoke(); // trigger event*/
    }
}
