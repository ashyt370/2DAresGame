using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;

    // Camera Settings
    private Camera camera;
    private Vector3 camOffset = new Vector3(0, 0, -10f);
    private float camSmoothSpeed = 10f;

    [Header("Input Actions Asset")]
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction lookAction;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;

        // Bind action map
        var actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        lookAction = actionMap.FindAction("Look");
    }

    private void FixedUpdate()
    {
        // Get input values
        moveDirection = moveAction.ReadValue<Vector2>();
        mousePosition = lookAction.ReadValue<Vector2>();

        // Move character
        rb.linearVelocity = moveDirection * moveSpeed;

        // Set Player face direction
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(mousePosition);
        Vector2 dir = mouseWorldPos - rb.position;

        // Set player rotation
        if (dir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = targetAngle;
        }
        FollowCamera();
    }

    private void FollowCamera()
    {
        Vector3 targetPos = transform.position + camOffset;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, camSmoothSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

}
