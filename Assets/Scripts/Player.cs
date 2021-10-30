using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public new Camera camera; // references player main camera
    CharacterController controller; // references player's characterController
    public LayerMask excludeMask; // ground mask

    [Header("Movement")]
    public float playerSpeed = 5f; // ground speed
    public float movementSharpness = 15f; // movement sharpness

    [Header("Camera")]
    public float zoomSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 12f;
    public float pitch = 1.25f;
    public float rotationSpeed = 1f;
    public Vector3 offset;
    float zoom = 3f;
    float yaw = 100f;

    Vector3 characterVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        CameraRotate();
    }

    void LateUpdate()
    {
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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, excludeMask)) // if raycast is successful
        {
            Vector3 targetDirection = hit.point - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0.0001f, newDirection.z), Vector3.up);
        }

        Vector3 move = GetMoveInput(); // WASD input
        Vector3 targetVelocity = move * playerSpeed; // gets users position and sets velocity
        characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpness * Time.deltaTime); // velocity
        controller.Move(characterVelocity * Time.deltaTime); // move character based on forces
    }
}
