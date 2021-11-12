using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("References")]
    public new Camera camera; // references main camera
    CharacterController controller; // references player's characterController
    public LayerMask groundMask; // ground mask
    public Transform weaponParent; // weapon slot

    [Header("Movement")]
    public float playerSpeed = 5f;
    public float movementSharpness = 15f;

    [Header("Camera")]
    public float zoomSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 12f;
    public float rotationSpeed = 1f;
    public Vector3 offset;
    float pitch = 1.25f;
    float zoom = 3f;
    float yaw = 100f;

    Vector3 characterVelocity;
    Inventory inventory;
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        // references
        inventory = Inventory.instance;
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        CameraRotate();
        CameraMovement();
    }

    void CameraRotate()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; // updates zoom
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom); // clamps zoom
    }

    void CameraMovement()
    {
        camera.transform.position = transform.position - offset * zoom; // moves camera
        camera.transform.LookAt(transform.position + Vector3.up * pitch); // aims camera at player
        camera.transform.RotateAround(transform.position, Vector3.up, yaw); // rotates camera
    }

    Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); // gets vector for x and z movement
        move = Vector3.ClampMagnitude(move, 1);
        return move;
    }

    void CharacterMovement()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); // gets ray based on mouse position on screen

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask)) // if raycast is successful
        {
            // targetDirection gets the direction towards target, newDirection is used so rotation isn't instant,
            // transform.rotation applies rotation
            Vector3 targetDirection = hit.point - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0.0001f, newDirection.z), Vector3.up);
        }

        Vector3 move = GetMoveInput(); // WASD input
        Vector3 targetVelocity = move * playerSpeed; // gets users position and sets velocity
        characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpness * Time.deltaTime); // velocity over time
        controller.Move(characterVelocity * Time.deltaTime); // move character based on forces
        transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
    }

    public Vector3 GetTileInfrontOfPlayer()
    {   // dir = direction player is facing
        // pos = player position rounded down to int
        // tile = tile infront of player
        Vector3 dir = transform.forward;
        Vector3 pos = new Vector3 (Mathf.FloorToInt(transform.position.x), 0f, Mathf.FloorToInt(transform.position.z));
        Vector3 tile = new Vector3(Mathf.Round(pos.x + dir.x), 0f, Mathf.Round(pos.z + dir.z));
        return tile;
    }
}
